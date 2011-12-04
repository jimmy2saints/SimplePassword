SimplePassword
======================

This is a simple class for creating salted password hashes.

Typical use would be:

	```c#
	var passwordHash = new SaltedPasswordHash("someClearTextPassword");
	SaveToDatebase(passwordHash.Hash, passwordHash.Salt);
	```
	
Later to verify the password

	```c#
	var hash = GetHashFromDatabase();
	var salt = GetSaltFromDatabase();
	var passwordHash = new SaltedPasswordHash(hash,salt);
	bool verified = passwordHash.Verify("someClearTextPassword");
	if(verified)
		// Log user in, or similar
	```
	
