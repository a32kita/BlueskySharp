# スクリプトが置いてあるディレクトリを基準に処理する
$baseDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# 対象ファイルの拡張子
$fileExtensions = @("*.cs", "*.csproj")

# 再帰的にすべての対象ファイルを取得
$files = Get-ChildItem -Path $baseDir -Recurse -Include $fileExtensions

Write-Output ($files.Length.ToString() + " files found.")

# 検索文字列と置換文字列
#$search = "EndPoints"
#$replace = "Endpoints"
$search = "EndPoint"
$replace = "Endpoint"

# 各ファイルを処理
foreach ($file in $files) {
    # ファイル内容をエンコーディングを指定して読み込む（例: UTF8）
    $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8

    # 指定した文字列を置換
    $newContent = $content -replace $search, $replace

    # 変更があれば上書き保存
    if ($content -cne $newContent) {
        # エンコーディングを指定して保存
        Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
        Write-Host "Updated: $($file.FullName)"
    }
}
