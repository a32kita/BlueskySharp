# �X�N���v�g���u���Ă���f�B���N�g������ɏ�������
$baseDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# �Ώۃt�@�C���̊g���q
$fileExtensions = @("*.cs", "*.csproj")

# �ċA�I�ɂ��ׂĂ̑Ώۃt�@�C�����擾
$files = Get-ChildItem -Path $baseDir -Recurse -Include $fileExtensions

Write-Output ($files.Length.ToString() + " files found.")

# ����������ƒu��������
#$search = "EndPoints"
#$replace = "Endpoints"
$search = "EndPoint"
$replace = "Endpoint"

# �e�t�@�C��������
foreach ($file in $files) {
    # �t�@�C�����e���G���R�[�f�B���O���w�肵�ēǂݍ��ށi��: UTF8�j
    $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8

    # �w�肵���������u��
    $newContent = $content -replace $search, $replace

    # �ύX������Ώ㏑���ۑ�
    if ($content -cne $newContent) {
        # �G���R�[�f�B���O���w�肵�ĕۑ�
        Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
        Write-Host "Updated: $($file.FullName)"
    }
}
