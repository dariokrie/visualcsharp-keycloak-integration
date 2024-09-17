# Configure and run Keycloak to run in Docker
```
docker run -p 8081:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:25.0.5 start-dev
```

## Log in to the Admin Console
1. Go to the Keycloak Admin Console (http://localhost:8081/admin).
2. Log in with the username and password you created earlier. (admin admin)

### Create a Realm
> A realm in Keycloak is equivalent to a tenant. Each realm allows an administrator to create isolated groups of applications and users. Initially, Keycloak includes a single realm, called master. Use this realm only for managing Keycloak and not for managing any applications.

Use these steps to create the first realm.

1. Open the Keycloak Admin Console.
2. Click Keycloak next to master realm, then click Create Realm.
3. Enter `myrealm` in the Realm name field.

Click Create.

### Create a User
> Initially, the realm has no users. Use these steps to create a user.

1. Verify that you are still in the `myrealm` realm, which is shown above the word Manage.
2. Click Users in the left-hand menu.
3. Click Add user.
4. Fill in the form with the following values:
	- Username: `myuser`
	- First name: any first name
	- Last name: any last name
5. Click Create.

This user needs a password to log in. To set the initial password:

1. Click Credentials at the top of the page.
2. Fill in the Set password form with a password.
3. Toggle Temporary to Off so that the user does not need to update this password at the first login.

### You can now log in to the Account Console to verify this user is configured correctly
1. Open the Keycloak Account Console (http://localhost:8081/realms/myrealm/account).
2. Log in with `myuser` and the password you created earlier.

As a user in the Account Console, you can manage your account including modifying your profile, adding two-factor authentication, and including identity provider accounts.

## Secure / Register Application
1. Open the Keycloak Admin Console.
2. Click the word master in the top-left corner, then click `myrealm`.
3. Click Clients.
4. Click Create client
5. Fill in the form with the following values:
6. Client type: OpenID Connect
7. Client ID: `myrealmclient`
8. Click Next
9. Confirm that Standard flow is enabled.
10. Click Next.
11. Make these changes under Login settings.
12. Set Valid redirect URIs to `*`
13. Set Web origins to `*`
14. Under Capability config, turn on the following things:
	1. Client authentication
	2. Authorization
	3. OAuth 2.0 Device Authorization Grant
	4. Direct access grants
15. Click Save.

> To confirm the client was created successfully, you can use the SPA testing application on the Keycloak website (https://www.keycloak.org/app/).

1. Open (https://www.keycloak.org/app/) .
2. Click Save to use the default configuration.
3. Click Sign in to authenticate to this application using the Keycloak server you started earlier

# Integrate Keycloak into ASP.NET Project

1. Open `appsettings.Development.json` and paste your client secret into the corresponding field.