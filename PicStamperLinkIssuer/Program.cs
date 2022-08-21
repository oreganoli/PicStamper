// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using LinkSigner;

var rsa = RSA.Create();
rsa.ImportFromPem(Config.PemKey);
var signer = new CloudfrontUrlSigner(rsa, "picstamper-intake.oreganoli.xyz", "testFolder", Config.KeyPairId);
Console.WriteLine(signer.ProduceUrl("PLACEHOLDER", 3600));