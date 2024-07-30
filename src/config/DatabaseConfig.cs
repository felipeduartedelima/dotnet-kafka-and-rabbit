namespace config;

public class DatabaseConfig
{
    public string DataSource { get; set; }
    public string UserID { get; set; }
    public string Password { get; set; }
    public string InitialCatalog { get; set; }
    public bool Encrypt { get; set; }
}