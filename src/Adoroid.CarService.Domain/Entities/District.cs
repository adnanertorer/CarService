namespace Adoroid.CarService.Domain.Entities;

public class District
{
    public District()
    {
        Companies = new HashSet<Company>();
    }
    public int Id { get; set; }
    public int CityId { get; set; }
    public string Name { get; set; }
    public string CityName { get; set; }

    public ICollection<Company>? Companies { get; set; }
}
