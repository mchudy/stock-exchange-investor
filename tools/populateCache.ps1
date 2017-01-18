# This script is used by an Azure WebJob and is invoked each night at 4am
# WARNING: It is not included in the build process on TeamCity. If changed, it must be deployed manually.
# By default runs on dev
# Examples:
# .\populateCache.ps1 -email a@gmail.com -password 123
# .\populateCache.ps1 -email a@gmail.com -password 123 -prod

Param(
  [Parameter(Mandatory=$true)]
  [string]$email,

  [Parameter(Mandatory=$true)]
  [string]$password,

  [switch]$prod,
  [string]$domain
)
If ([string]::IsNullOrEmpty($domain)) {
    If($prod) {
        $domain = "ministockexchange.azurewebsites.net"
    } Else {
        $domain = "ministockexchangedev.azurewebsites.net"
    }
}

$baseAddress = "https://" + $domain + "/"

# do not display UI (WebJob sandbox restriction)
$progressPreference = "silentlyContinue"

# load login page
Write-Output "Logging in to the website"
$loginUrl = $baseAddress + "Account/Login"
$ws = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$loginResponse = Invoke-WebRequest $loginUrl -WebSession $ws -UseBasicParsing
$requestVerificationToken  = ($loginResponse.InputFields | Where-Object { $_.name -eq "__RequestVerificationToken" }).value
$body = @{
    "__RequestVerificationToken" = $requestVerificationToken;
    "Email" = $email;
    "Password" = $password;
}

$cookie = New-Object System.Net.Cookie 
$cookie.Name = "__RequestVerificationToken"
$cookie.Value = $requestVerificationToken
$cookie.Domain = $domain
$ws.Cookies.Add($cookie)

Invoke-WebRequest $loginUrl -Body $body -Method POST -WebSession $ws -UseBasicParsing > $null
If(-not $ws.Cookies.GetCookies($loginUrl)[".AspNet.ApplicationCookie"]) {
    Throw "Login failed"
}
Write-Output "Logged in successfully"

Write-Output "Invoking page requests"
Write-Output "  Dashboard"
Invoke-WebRequest ($baseAddress + "Dashboard") -WebSession $ws -UseBasicParsing > $null
Write-Output "  Charts"
Invoke-WebRequest ($baseAddress + "Charts") -WebSession $ws -UseBasicParsing > $null
Write-Output "  Wallet"
Invoke-WebRequest ($baseAddress + "Wallet") -WebSession $ws -UseBasicParsing > $null
Write-Output "  Strategies"
Invoke-WebRequest ($baseAddress + "Strategies") -WebSession $ws -UseBasicParsing > $null
Invoke-WebRequest ($baseAddress + "Strategies/EditStrategy") -WebSession $ws -UseBasicParsing > $null
Write-Output "  Simulations"
Invoke-WebRequest ($baseAddress + "Simulations/RunSimulation") -WebSession $ws -UseBasicParsing > $null

Write-Output "Invoking AJAX tables requests"
Write-Output "  TodaySignalsTable"
$json = @"
{"draw":1,"columns":[{"data":"Company","name":"Company","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Indicator","name":"Indicator","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Action","name":"Action","searchable":true,"orderable":false,"search":{"value":"","regex":false}}],"order":[],"start":0,"length":15,"search":{"value":"","regex":false}}
"@
Invoke-WebRequest ($baseAddress + "Dashboard/GetTodaySignalsTable") -Body $json -Method POST -WebSession $ws -ContentType "application/json" -UseBasicParsing > $null

Write-Output "  GetOwnedStocksTable"
$json = @'
{"draw":1,"columns":[{"data":"CompanyName","name":"CompanyName","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"OwnedStocksCount","name":"OwnedStocksCount","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"AverageBuyPrice","name":"AverageBuyPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"TotalBuyPrice","name":"TotalBuyPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"CurrentPrice","name":"CurrentPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"CurrentValue","name":"CurrentValue","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Profit","name":"Profit","searchable":true,"orderable":false,"search":{"value":"","regex":false}}],"order":[],"start":0,"length":10,"search":{"value":"","regex":false}}
'@
Invoke-WebRequest ($baseAddress + "Dashboard/GetOwnedStocksTable") -Body $json -Method POST -WebSession $ws -ContentType "application/json" -UseBasicParsing > $null

Write-Output "  GetOwnedStocksTable"
$json = @'
{"draw":1,"columns":[{"data":"CompanyName","name":"CompanyName","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"OwnedStocksCount","name":"OwnedStocksCount","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"AverageBuyPrice","name":"AverageBuyPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"TotalBuyPrice","name":"TotalBuyPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"CurrentPrice","name":"CurrentPrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"CurrentValue","name":"CurrentValue","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Profit","name":"Profit","searchable":true,"orderable":false,"search":{"value":"","regex":false}}],"order":[],"start":0,"length":10,"search":{"value":"","regex":false}}
'@
Invoke-WebRequest ($baseAddress + "Wallet/GetCurrentTransactionsTable") -Body $json -Method POST -WebSession $ws -ContentType "application/json" -UseBasicParsing > $null

Write-Output "  GetMostActive"
$json = @'
{"draw":1,"columns":[{"data":"CompanyName","name":"CompanyName","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"ClosePrice","name":"ClosePrice","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Change","name":"Change","searchable":true,"orderable":false,"search":{"value":"","regex":false}},{"data":"Volume","name":"Volume","searchable":true,"orderable":false,"search":{"value":"","regex":false}}],"order":[],"start":0,"length":10,"search":{"value":"","regex":false}}
'@
Invoke-WebRequest ($baseAddress + "Dashboard/GetMostActiveTable") -Body $json -Method POST -WebSession $ws -ContentType "application/json" -UseBasicParsing > $null

Write-Output "Finished"