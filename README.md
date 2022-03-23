# Wasabi compliance: Delete after retention editor

Use the tool to update the Delete After Retention flag on any bucket that have compliance enabled.

## Prerequisite:

- [Dotnet](https://dotnet.microsoft.com/en-us/download)

## How to run?

Under the _Publishes_ folder, you will find a DLL file and an executable (windows users only).
You can either run the exe file, or the dll file via the dotnet cli using this command:

```
dotnet Compliance.dll
```

## Execution:

1. Enter your Access key ID
2. Enter your Secret Key
3. Enter your bucket name
4. Enter the region of your bucket (by default it's **us-east-1**)

> The region needs to be one of the listed regions at the start of the script!
> In case you specify a wrong region, you will get a 403 error!
> In case the bucket does not exist in your Wasabi account, a 404 error will be shown in the screen

![alt text](screenshots/Execution.png)

# Want to improve it?

Feel free to submit your pull-request!
