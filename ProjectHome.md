Windows Command line regular expression file content replace tool.

```
Usage CmdRplc.exe filename/mask search replace /m /i /s /test /prefix=replaced_
        /m RegexOptions.Multiline
        /i RegexOptions.IgnoreCase
        /s RegexOptions.Singleline
        /e RegexOptions.ECMAScript
        /test test mode, no actual replacement
        /prefix=<some string prefix for the output file>
```

/m /i /s /test /prefix= are optional.
If prefix is not specified - original file will be placed into the %temp%\cmdrplc\ directory upon content replacement.

Requires .net framework.