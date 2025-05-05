using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;

class Program
{
    static async Task Main(string[] args)
    {

        var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>();

        var configuration = builder.Build();
        string nodeUrl = configuration["nodeUrl"];

        // Dirección del nodo Ethereum (puede ser Infura, Alchemy, etc.)
        
        var web3 = new Web3(nodeUrl);

        // Hash de la transacción que quieres consultar
        var txHash = "0x2a3dfc715a1b6f107d3d1d548b0d03682440ff8435d17b0ca1fcc34aa7e98cce";

        // Obtener el recibo de la transacción
        var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);

        if (receipt != null)
        {
            Console.WriteLine($"Transaction Hash: {receipt.TransactionHash}");
            Console.WriteLine($"Block Number (hex): {receipt.BlockNumber.Value.ToString("X")}");
            Console.WriteLine($"Block Number (decimal): {receipt.BlockNumber.Value}");
            Console.WriteLine($"Status: {receipt.Status.Value}");
        }
        else
        {
            Console.WriteLine("Transaction receipt not found (may not be mined yet).");
        }
    }
}
