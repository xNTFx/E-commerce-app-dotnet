using E_commerce_app_dotnet.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly IConfiguration _configuration;

        public FirebaseAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var clientEmail = _configuration["Google:ClientEmail"];
                var privateKey = _configuration["Google:PrivateKey"];
                var projectId = _configuration["Google:ProjectId"];

                if (string.IsNullOrWhiteSpace(clientEmail) ||
                    string.IsNullOrWhiteSpace(privateKey) ||
                    string.IsNullOrWhiteSpace(projectId))
                {
                    throw new InvalidOperationException("Firebase configuration is missing.");
                }

                var formattedPrivateKey = privateKey.Replace("\\n", "\n");
                string json = $"{{\"type\": \"service_account\", \"project_id\": \"{projectId}\", \"private_key_id\": \"\", \"private_key\": \"{formattedPrivateKey}\", \"client_email\": \"{clientEmail}\", \"client_id\": \"\", \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\", \"token_uri\": \"https://oauth2.googleapis.com/token\", \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\", \"client_x509_cert_url\": \"\"}}";

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(json),
                    ProjectId = projectId
                });
            }
        }

        public async Task<string> VerifyIdTokenAsync(string idToken)
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;
        }

        public string VerifyIdToken(string idToken)
        {
            return VerifyIdTokenAsync(idToken).GetAwaiter().GetResult();
        }
    }
}
