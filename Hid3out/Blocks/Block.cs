using System.Security.Cryptography;
using System.Text;
using Hid3out.Tx;

namespace Hid3out.Blocks;

public class Block
{
    private readonly DateTime _timeStamp;
    
    private long _nonce;
    
    public string PreviousHash { get; set; }
    public List<Transaction> Transactions { get; set; }
    
    public string Hash { get; private set; }
    
    public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
    {
        _timeStamp = timeStamp;
        _nonce = 0;
        Transactions = transactions;
        PreviousHash = previousHash;
        Hash = CreateHash();
    }
    
    public void MineBlock(int proofOfWorkDifficulty)
    {
        var hashValidationTemplate = new string('0', proofOfWorkDifficulty);
        
        while (Hash[..proofOfWorkDifficulty] != hashValidationTemplate)
        {
            _nonce++;
            Hash = CreateHash();
        }
        Console.WriteLine("Blocked with HASH={0} successfully mined!", Hash);
    }
    
    public string CreateHash()
    {
        using var sha256 = SHA256.Create();
        var rawData = PreviousHash + _timeStamp + Transactions + _nonce;
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Encoding.Default.GetString(bytes);
    }
}