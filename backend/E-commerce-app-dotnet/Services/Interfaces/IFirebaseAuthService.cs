using System.Threading.Tasks;

namespace E_commerce_app_dotnet.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        /// Verifies the provided Firebase ID token asynchronously.
        Task<string> VerifyIdTokenAsync(string idToken);

        /// Verifies the provided Firebase ID token.
        string VerifyIdToken(string idToken);
    }
}
