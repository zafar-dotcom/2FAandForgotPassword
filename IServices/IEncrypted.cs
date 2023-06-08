namespace IPMO.IServices
{
    public  interface IEncrypted
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
        byte[] GenerateEncryptionKey();
        //byte[] FixedIV();
        byte[] DecryptPassword(byte[] encryptedBytes);
        byte[] EncryptPassword(byte[] clearBytes);
    }
}
