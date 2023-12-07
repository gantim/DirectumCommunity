namespace DirectumCommunity.Models.Responses;

public class PersonsResponse
{
    public string ODataContext { get; set; }
    public List<Person> Value { get; set; }
}