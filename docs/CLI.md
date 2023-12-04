---
hide_table_of_contents: true
---
Use the `dotnet csharpier` command to run CSharpier from the command line.

In practice, it will look something like:
```
dotnet csharpier .
```
This command will format all c# files in the current directory and its children.

You may want to set up an [ignore file](Ignore.md) or [configuration file](Configuration.md).

### Command Line Options
```console
Usage:
  dotnet-csharpier [options] [<directoryOrFile>]

Arguments:
  <directoryOrFile>    One or more paths to a directory containing files to format or a file to format. If a path is not specified the current directory is used

Options:
  --check           Check that files are formatted. Will not write any changes.
  --loglevel        Specify the log level - Debug, Information (default), Warning, Error, None
  --no-cache        Bypass the cache to determine if a file needs to be formatted.
  --fast            Skip comparing syntax tree of formatted file to original file to validate changes.
  --skip-write      Skip writing changes. Generally used for testing to ensure csharpier doesn't throw any errors or cause syntax tree validation failures.
  --write-stdout    Write the results of formatting any files to stdout.
  --pipe-multiple-files  Keep csharpier running so that multiples files can be piped to it via stdin
  --config-path     Path to the CSharpier configuration file
  --version         Show version information
  -?, -h, --help    Show help and usage information


```

### \[<directoryOrFile\>]
 
If a list of paths is supplied
- if the path points to an existing file, CSharpier will format that file
- if the path points to an existing directory, CSharpier will recursively format the contents of that directory

If a list of paths is not supplied, then stdin is read.

### --check
Used to check if your files are already formatted. Outputs any files that have not already been formatted.
This will return exit code 1 if there are unformatted files which is useful for CI pipelines.

### --loglevel
Changes the level of logging output. Valid options are:
- None
- Error
- Warning
- Information (default)
- Debug


### --no-cache
_First available in 0.19.0_

This option can be used to bypass the cache that is normally used to speed up formatting files.  
By default the following are used as cache keys and a file is only formatted if one of them has changed.
* CSharpier Version
* CSharpier Options
* Content of the file

The cache is stored at `[LocalApplicationData]/CSharpier/.formattingCache`.

### --fast
CSharpier validates the changes it makes to a file.
It does this by comparing the syntax tree before and after formatting, but ignoring any whitespace trivia in the syntax tree.
If a file fails validation, CSharpier will output the lines that differ. If this happens it indicates a bug in CSharpier's code.  
This validation may be skipped by passing the --fast argument.

An example of CSharpier finding a file that failed validation.
```
\src\[Snip]\AbstractReferenceFinder_GlobalSuppressions.cs       - failed syntax tree validation
    Original: Around Line 280
            }

            if (prefix.Span[^2] is < 'A' or > 'Z')
            {
                return false;
            }

            if (prefix.Span[^1] is not ':')
    Formatted: Around Line 330
            }

            if (prefix.Span[^2] is )
            {
                return false;
            }

            if (prefix.Span[^1] is not ':')
```

### --write-stdout
By default CSharpier will format files in place. This option allows you to write the formatting results to stdout.

If you pipe input to CSharpier it will also write the formatting results to stdout.

*TestFile.cs*
```c#
public class ClassName
{
    public string Field;
}
```
*shell*
```bash
$ cat TestFile.cs | dotnet csharpier
public class ClassName
{
    public string Field;
}
```

### --pipe-multiple-files
Running csharpier to format a single file is slow because of the overhead of starting up dotnet. 
This option keeps csharpier running so that multiple files can be formatted. This is mainly used by IDE plugins
to drastically improve formatting time.  
The input is a '\u0003' delimited list of file names followed by file contents.  
The results are written to stdout delimited by \u0003.  
For an example of implementing this in code see [this example](https://github.com/belav/csharpier/blob/main/Src/CSharpier.VSCode/src/CSharpierProcessPipeMultipleFiles.ts)
```bash
$ [FullPathToFile]\u0003[FileContents]\u0003[FullPathToFile]\u0003[FileContents]\u0003 | dotnet csharpier --pipe-multiple-files
public class ClassName
{
    public string Field;
}
\u0003
public class ClassName
{
    public string Field;
}
```

### --config-path
_First available in 0.23.0_
If your configuration file lives in a location that CSharpier would not normally resolve it (such as in a config folder) 
you can pass the path for the configuration file to CSharpier.
```bash
dotnet csharpier . --config-path "./config/.csharpierrc"

# also supports any name for the config file
dotnet csharpier . --config-path "./config/csharpier.yaml"
```