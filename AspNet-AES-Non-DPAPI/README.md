# ASP.NET Core AES Encryption/Decryption without Data Protection APIs

## Goal

To solve the problems that Data Protection APIs introduce:
1. Key generation on diffrent machines cause decryption failure (invalid payload or key not found on key ring)
2. By default key rotation is 90 days, don't want this
