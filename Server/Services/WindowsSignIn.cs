using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.DirectoryServices.AccountManagement;

namespace Server.Services
{
    public class WindowsSignIn
    {
		/// <summary>
		/// Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination
		/// as an asynchronous operation.
		/// </summary>
		/// <param name="userName">The user name to sign in.</param>
		/// <param name="password">The password to attempt to sign in with.</param>
		/// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
		/// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
		/// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
		/// for the sign-in attempt.</returns>
		public async Task<SignInResult> PasswordSignInAsync(string userName, string password,
			bool isPersistent, bool lockoutOnFailure)
		{
			//var user = await UserManager.FindByNameAsync(userName);
			//if (user == null)
			//{
			//    return SignInResult.Failed;
			//}

			//return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
			return await Task.Run(() => IsAuthorizedUser(userName, password));
		}

#pragma warning disable CA1416 // Validate platform compatibility
		private SignInResult IsAuthorizedUser(string username, string password)
		{
			const string validRoleMseTestSoftware = "OP AT OCS Manufacturing Systems Engineering Software Manufacturi";
			var domainName = Environment.UserDomainName; // -> FRONIUS

			// Is valid user
			using var ctx = new PrincipalContext(ContextType.Domain, domainName);
			// check validity of given credentials
			if (!ctx.ValidateCredentials(username.ToUpper(), password,
				ContextOptions.Negotiate)) return SignInResult.Failed;
			// get user information
			using var userPrincipial = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName,
				username);
			if (userPrincipial == null) return SignInResult.Failed;
			// get assigned group/roles
			using var userGroups = userPrincipial.GetGroups(ctx);
            return userGroups.Any(x => x.Name.Equals(validRoleMseTestSoftware))
				? SignInResult.Success
				: SignInResult.Failed;
		}
#pragma warning restore CA1416 // Validate platform compatibility
	}
}
