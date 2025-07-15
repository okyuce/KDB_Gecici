namespace Csb.YerindeDonusum.Domain.Addons.FileAddons;

public abstract class FileType
{
    protected string Name { get; set; }

    protected string Description { get; set; }

    protected string FileExtension { get; set; }

    public List<string> Extensions { get; } = new List<string>();

    public List<byte[]> Signatures { get; } = new List<byte[]>();

    public int SignatureLength => Signatures.Max(m => m.Length);

    protected FileType AddSignatures(params byte[][] bytes)
    {
        Signatures.AddRange(bytes);

        return this;
    }

    protected FileType AddExtensions(params string[] extensions)
    {
        Extensions.AddRange(extensions);
        return this;
    }

    public FileTypeVerifyResult Verify(Stream stream)
    {
        stream.Position = 0;
        var reader = new BinaryReader(stream);
        var headerBytes = reader.ReadBytes(SignatureLength);

        return Verify(headerBytes);

        #region ...: alternative code :...
        //return new FileTypeVerifyResult
        //{
        //    Name = Name,
        //    Description = Description,
        //    IsVerified = Signatures.Any(signature =>
        //        headerBytes.Take(signature.Length)
        //            .SequenceEqual(signature)
        //    )
        //};
        #endregion
    }

    public FileTypeVerifyResult Verify(byte[] bytes)
    {
        return new FileTypeVerifyResult
        {
            Name = Name,
            Description = Description,
            IsVerified = Signatures.Any(signature =>
                bytes.Take(signature.Length)
                    .SequenceEqual(signature)
            )
        };
    }
}

public class FileTypeVerifyResult
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileExtension { get; set; }
    public bool IsVerified { get; set; }
}
