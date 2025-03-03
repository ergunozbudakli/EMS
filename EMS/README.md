Çalışan Yönetim Sistemi (EMS)

Projenin amacı şirketinizin departman ve çalışan bilgilerini yönetmektir.

Çalışan Yönetimi
- Yeni çalışan ekleyin, bilgilerini güncelleyin veya çıkış yapanları sistemden kaldırın
- Her çalışan için detaylı bilgileri (isim, e-posta, telefon, maaş) saklayın
- İstediğiniz kriterlere göre çalışanları filtreleyip arayın
- Tüm çalışanlarınızı tek ekranda görüntüleyin

Departman Yönetimi
- Yeni departmanlar oluşturulup düzenlenebilmekte
- Her departmandaki çalışanlar görülebilmekte
- Departmanlara özel istatistik verileri incelenebilmekte
  * Kaç kişi çalışıyor?
  * Ortalama maaş ne kadar?

Kullandığım Teknolojiler

- ASP.NET Core 8.0 
- Entity Framework Core 8.0 
- MS SQL Server 
- Bootstrap 5 
- DataTables
- SweetAlert2
- Filtreleme işlemlerinde LINQ yapısını kullandım

Kurulum

1. Önce NuGet Paketlerini Yükleyin
Proje içinde kullanılan paketler:

<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />


2. Veritabanı Bağlantı Ayarları
`appsettings.json` dosyasını kendi veritabanı bilgilerinize göre düzenleyin:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=host;Database=EMS;User ID=Kullanıcı;Password=Şifre;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False"
  }
}


3. Veritabanını Hazırlayın
Sırasıyla şu adımları takip edin:


- Eski migration'ları temizleyin (varsa)
dotnet ef migrations remove --force

- Ana migration'ı oluşturun
dotnet ef migrations add InitialCreate

- İstatistik sorgularını ekleyin
dotnet ef migrations add AddDepartmentStatisticsProcedure


İstatistik sorguları için oluşturduğunuz migration dosyasına (`AddDepartmentStatisticsProcedure.cs`) şu kodu ekleyin:

protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        DROP PROCEDURE IF EXISTS sp_GetDepartmentStatistics;
        GO

        CREATE PROCEDURE sp_GetDepartmentStatistics
        AS
        BEGIN
            SELECT 
                d.Id AS DepartmentId,
                d.Name AS DepartmentName,
                COUNT(e.Id) AS EmployeeCount,
                ISNULL(AVG(CAST(e.Salary AS DECIMAL(18,2))),0) AS AverageSalary
            FROM Departments d
            LEFT JOIN Employees e ON d.Id = e.DepartmentId
            GROUP BY d.Id, d.Name
            ORDER BY d.Name;
        END
    ");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetDepartmentStatistics");
}


Son olarak veritabanını oluşturun:

- Eski veritabanını temizleyin
dotnet ef database drop -f

- Yeni veritabanını oluşturun
dotnet ef database update


Departman İstatistikleri Nasıl Çalışır?

API endpoint'ini kullanarak:

GET /api/reports/department-statistics


Örnek çıktı:

[
  {
    "departmentId": 1,
    "departmentName": "İnsan Kaynakları",
    "employeeCount": 5,
    "averageSalary": 12500.00
  },
  {
    "departmentName": "Bilgi Teknolojileri",
    "employeeCount": 8,
    "averageSalary": 15750.50
  }
]




