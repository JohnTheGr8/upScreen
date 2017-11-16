upScreen [![Build status](https://ci.appveyor.com/api/projects/status/3mxesarcx888e91o?svg=true)](https://ci.appveyor.com/project/JohnTheGr8/upscreen)
=====

[Changelog][changelog] | [GPLv3][gpl] | [Issues][issues]

### About

Easily capture your screen and upload the image to your own server.

### Features
- Capture specific area of screen
- Capture specific Windows
- Capture image from clipboard
- Upload image files from the context menu
- Capture full screen
- Upload image to your server, via FTP or SFTP
- Switch between multiple accounts/folders
- Open image file in browser
- Copy image link to clipboard
- Multiple monitor support

### Dependencies

| Library                           | Description                                      | License   |
|:---------------------------------:|:-------------------------------------------------|:---------:|
| [FluentFTP][fluentftp]            | The FTP library                                  | MIT       |
| [SSH.NET][sshnet]                 | The SFTP library                                 | MIT       |
| [Json.NET][jsonnet]               | The json library used for the configuration file | MIT       |
| [Squirrel.Windows][squirrel]      | Installation/update tool                         | MIT       |
| [Renci.SshNet.Async][sshnetasync] | Async extensions for SSH.NET                     | MIT       |

[fluentftp]: https://github.com/hgupta9/FluentFTP/
[sshnet]: https://github.com/sshnet/SSH.NET/
[sshnetasync]: https://github.com/JohnTheGr8/Renci.SshNet.Async
[jsonnet]: https://github.com/JamesNK/Newtonsoft.Json/
[squirrel]:https://github.com/Squirrel/Squirrel.Windows
[gpl]: http://www.tldrlegal.com/license/gnu-general-public-license-v3-(gpl-3)
[changelog]: https://github.com/JohnTheGr8/upScreen/blob/master/CHANGELOG.md
[issues]: https://github.com/JohnTheGr8/upScreen/issues