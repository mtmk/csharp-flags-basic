"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe" /p:configuration=Release msvs\FlagsBasic.sln

dotnet build -c Release FlagsBasic

rd /q /s dist

md dist
md dist\lib

md dist\lib\netstandard2.0
copy FlagsBasic\bin\Release\netstandard2.0\FlagsBasic.dll dist\lib\netstandard2.0
copy FlagsBasic\bin\Release\netstandard2.0\FlagsBasic.pdb dist\lib\netstandard2.0

md dist\lib\net47
copy msvs\FlagsBasic47\bin\Release\* dist\lib\net47

md dist\lib\net46
copy msvs\FlagsBasic46\bin\Release\* dist\lib\net46

md dist\lib\net45
copy msvs\FlagsBasic45\bin\Release\* dist\lib\net45

md dist\lib\net35
copy msvs\FlagsBasic35\bin\Release\* dist\lib\net35

md dist\lib\net20
copy msvs\FlagsBasic20\bin\Release\* dist\lib\net20

copy FlagsBasic.nuspec dist

cd dist

nuget pack

nuget push -source nuget.org FlagsBasic.1.0.1.nupkg %1

