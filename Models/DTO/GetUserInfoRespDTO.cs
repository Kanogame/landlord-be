public class GetUserInfoRespDTO
{
    public string FullName { get; set; } = string.Empty;
    public int ActivePropertiesCount { get; set; }
    public int RentedSoldRentEndingCount { get; set; }
    public string Experience { get; set; } = string.Empty;
}
