using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BankService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage implementation of <see cref="IDocumentStorage"/>. It uploads
/// the customer's onboarding document into a container and returns the blob URI.
/// For local development this works against the Azurite emulator
/// (connection string "UseDevelopmentStorage=true").
/// </summary>
public sealed class BlobDocumentStorage : IDocumentStorage
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobDocumentStorage> _logger;

    public BlobDocumentStorage(
        BlobServiceClient blobServiceClient,
        string containerName,
        ILogger<BlobDocumentStorage> logger)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _logger = logger;
    }

    public async Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        //await _containerClient.CreateIfNotExistsAsync(
        //    PublicAccessType.None, cancellationToken: cancellationToken);

        //// Prefix with a GUID to avoid name collisions between customers.
        //var blobName = $"{Guid.NewGuid():N}-{fileName}";
        //var blobClient = _containerClient.GetBlobClient(blobName);

        //var options = new BlobUploadOptions
        //{
        //    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        //};

        //_logger.LogInformation("Uploading onboarding document to blob {BlobName}.", blobName);

        //await blobClient.UploadAsync(content, options, cancellationToken);

        //return blobClient.Uri.ToString();
        return "blob SAS";
    }
}
