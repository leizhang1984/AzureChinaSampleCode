1.仔细配置App.config

2.配置App.config文件的thumbprint，请在已经安装Azure PowerShell的机器，运行
Get-AzurePublishSettingsFile -Environment AzureChinaCloud

3.在弹出的IE界面中，输入Azure用户名和密码

4.将publishsettings文件拷贝到磁盘本地，然后用记事本打开，拷贝ManagementCertificate后面的一长字符串
注意这个字符串不能泄露

3.将包含windows service exe的目录拷贝到C盘

4.导航到.net framework安装目录
C:\Windows\Microsoft.NET\Framework\v4.0.30319

5.安装Windows Service
InstallUtil.exe  Path/WinServiceName.exe

//InstallUtil.exe D:\Work\Doc\FY16\Customer\Yum\20150216DMB\InstallService\YumDMBWindowsService.exe

//InstallUtil.exe C:\Yum!DMB\YumDMBWindowsService.exe

6.删除Windows Service
installutil /u D:\Work\Doc\FY16\Customer\Yum\20150216DMB\InstallService\YumDMBWindowsService.exe

//InstallUtil.exe /u C:\Yum!DMB\YumDMBWindowsService.exe