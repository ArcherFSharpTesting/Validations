
/// <summary>
/// Provides helpers for golden master (approval) testing, including file writers, namers, and approvers.
/// </summary>
module Archer.Validations.GoldenMaster

open System.IO
open ApprovalTests.Core
open Archer
open Archer.Validations.Types.Internal

/// <summary>
/// URL for ApprovalTests.Net project, used for attribution.
/// </summary>
let thanksUrl = "https://github.com/approvals/ApprovalTests.Net/"

let private writeTo fullPath writer result =
    Directory.CreateDirectory (Path.GetDirectoryName (fullPath : string)) |> ignore
    do writer fullPath result
    fullPath

let private writeBinaryTo fullPath result =
    let writer path (toWrite: byte array) = File.WriteAllBytes (path, toWrite)
    result |> writeTo fullPath writer

let private writeTextTo fullPath result =
    let writer path (toWrite: string) = File.WriteAllText (path, toWrite, System.Text.Encoding.UTF8)
    result |> writeTo fullPath writer
    
/// <summary>
/// Gets an <c>IApprovalWriter</c> for writing string results to text files.
/// </summary>
/// <param name="result">The string result to write.</param>
/// <returns>An <c>IApprovalWriter</c> for string results.</returns>
let getStringFileWriter result = 
    { new IApprovalWriter with 
        member _.GetApprovalFilename(baseName) = $"%s{baseName}.approved.txt"
        member _.GetReceivedFilename(baseName) = $"%s{baseName}.received.txt"
        member _.WriteReceivedFile(fullPathForReceivedFile) = 
            result |> writeTextTo fullPathForReceivedFile
    }

/// <summary>
/// Gets an <c>IApprovalWriter</c> for writing binary results to files with a custom extension.
/// </summary>
/// <param name="extensionWithoutDot">The file extension (without dot).</param>
/// <param name="result">The binary data to write.</param>
/// <returns>An <c>IApprovalWriter</c> for binary results.</returns>
let getBinaryFileWriter extensionWithoutDot result =
    { new IApprovalWriter with
        member _.GetApprovalFilename(baseName) = $"%s{baseName}.approved.%s{extensionWithoutDot}"
        member _.GetReceivedFilename(baseName) = $"%s{baseName}.received.%s{extensionWithoutDot}"
        member _.WriteReceivedFile fullPathForReceivedFile = 
            result |> writeBinaryTo fullPathForReceivedFile
    }

/// <summary>
/// Gets an <c>IApprovalWriter</c> for writing a stream as binary data to a file with a custom extension.
/// </summary>
/// <param name="extensionWithoutDot">The file extension (without dot).</param>
/// <param name="result">The stream to write.</param>
/// <returns>An <c>IApprovalWriter</c> for the stream data.</returns>
let getBinaryStreamWriter extensionWithoutDot (result:Stream) =
    let length = int result.Length
    let data : byte array = Array.zeroCreate length

    result.Read(data, 0, data.Length) |> ignore
    getBinaryFileWriter extensionWithoutDot data
    
/// <summary>
/// Canonicalizes a string for use in file names by removing or replacing invalid characters.
/// </summary>
/// <param name="value">The string to canonicalize.</param>
/// <returns>The canonicalized string.</returns>
let canonicalizeString (value: string) =
    let toString : char seq -> string = Seq.map string >> String.concat ""
    let canonicalized =
        value 
        |> Seq.filter (fun c -> 
            (System.Char.IsLetterOrDigit c)
            || c = ' '
            || c = '_'
            || c = '.'
            || c = '-'
        )
        |> Seq.map (fun c -> 
            if c = ' '
            then '_'
            else c
        )
        
    (canonicalized |> toString).Trim()    

let private getNamer (testInfo: ITestInfo) = 
    let path = testInfo.Location.FilePath
    let canonicalizedContainerRoot = testInfo.ContainerPath |> canonicalizeString
    let canonicalizedContainerName = testInfo.ContainerName |> canonicalizeString
    let canonicalizedName = testInfo.TestName |> canonicalizeString
    
    { new IGoldMasterNamer with
        member _.CanonicalizedContainerRoot with get () = canonicalizedContainerRoot
        member _.CanonicalizedContainerName with get () = canonicalizedContainerName
        member _.CanonicalizedTestName with get () = canonicalizedName
        member _.SourcePath with get () = path
        member _.Name with get () = $"%s{canonicalizedContainerName}.%s{canonicalizedName}"
    }
    
let private getGoldMasterApprover (goldMasterNamer: IGoldMasterNamer) (approver: IApprovalApprover) =  
    { new IGoldMasterApprover with
        member _.CanonicalizedContainerRoot with get () = goldMasterNamer.CanonicalizedContainerRoot
        member _.CanonicalizedContainerName with get () = goldMasterNamer.CanonicalizedContainerName
        member _.CanonicalizedTestName with get () = goldMasterNamer.CanonicalizedTestName
        member _.SourcePath with get () = goldMasterNamer.SourcePath
        member _.Name with get () = goldMasterNamer.Name
        member _.Approve () = approver.Approve ()
        member _.CleanUpAfterSuccess reporter = approver.CleanUpAfterSuccess reporter
        member _.Fail () = approver.Fail ()
        member _.ReportFailure reporter = approver.ReportFailure reporter
    }
    
/// <summary>
/// Gets a gold master approver for string results.
/// </summary>
/// <param name="testInfo">The test information.</param>
/// <param name="result">The string result to approve.</param>
/// <returns>An <c>IGoldMasterApprover</c> for string results.</returns>
let getStringFileApprover testInfo result =
    let goldMasterNamer = getNamer testInfo
    let approver = ApprovalTests.Approvers.FileApprover (getStringFileWriter result, goldMasterNamer, true)
    
    getGoldMasterApprover goldMasterNamer approver

/// <summary>
/// Gets a gold master approver for binary results.
/// </summary>
/// <param name="testInfo">The test information.</param>
/// <param name="extensionWithoutDot">The file extension (without dot).</param>
/// <param name="result">The binary data to approve.</param>
/// <returns>An <c>IGoldMasterApprover</c> for binary results.</returns>
let getBinaryFileApprover testInfo extensionWithoutDot result =
    let goldMasterNamer = getNamer testInfo
    let approver = ApprovalTests.Approvers.FileApprover (getBinaryFileWriter extensionWithoutDot result, goldMasterNamer)
    
    getGoldMasterApprover goldMasterNamer approver
    
        
/// <summary>
/// Gets a gold master approver for stream results.
/// </summary>
/// <param name="testInfo">The test information.</param>
/// <param name="extensionWithoutDot">The file extension (without dot).</param>
/// <param name="result">The stream to approve.</param>
/// <returns>An <c>IGoldMasterApprover</c> for stream results.</returns>
let getStreamFileApprover testInfo extensionWithoutDot (result:Stream) =
    let goldMasterNamer = getNamer testInfo
    let approver = ApprovalTests.Approvers.FileApprover (getBinaryStreamWriter extensionWithoutDot result, goldMasterNamer)
    
    getGoldMasterApprover goldMasterNamer approver
        
/// <summary>
/// Approves a test result using the provided reporter and approver, returning a test result.
/// </summary>
/// <param name="fullPath">The file path where the approval is performed.</param>
/// <param name="lineNumber">The line number where the approval is performed.</param>
/// <param name="reporter">The approval failure reporter.</param>
/// <param name="approver">The gold master approver.</param>
/// <returns><c>TestSuccess</c> if approved; otherwise, a validation failure result.</returns>
let approve fullPath lineNumber (reporter: IApprovalFailureReporter) (approver: IGoldMasterApprover) =
    if approver.Approve ()
    then
        do approver.CleanUpAfterSuccess reporter 
        TestSuccess
    else 
        do approver.ReportFailure reporter
                                                            
        match reporter with
        | :? IReporterWithApprovalPower as approvalReporter -> 
            if approvalReporter.ApprovedWhenReported ()
            then do approver.CleanUpAfterSuccess(reporter)
            ()
        | _ -> ()
        
        let fb = TestResultFailureBuilder id
        fb.ValidationFailure ({ ExpectedValue = $"%s{approver.Name}.approved"; ActualValue = $"%s{approver.Name}.received" }, fullPath, lineNumber)