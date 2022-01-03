using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

const string KEY_VAULT_URI = @"https://kvstudyplanner.vault.azure.net/";

var client = new SecretClient(new Uri(KEY_VAULT_URI), new DefaultAzureCredential());

const string SECRET_NAME = "cdbhandbooksecretconnectionstring";
var secret = await client.GetSecretAsync(SECRET_NAME);

System.Console.WriteLine(secret.Value);