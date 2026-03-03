Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    (Get-Content $_.FullName) `
        -replace 'Version="[^"]*"', '' |
        Set-Content $_.FullName
}
