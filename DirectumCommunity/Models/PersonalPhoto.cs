using System.ComponentModel.DataAnnotations.Schema;

namespace DirectumCommunity.Models;

public class PersonalPhoto
{
    public int Id { get; set; }
    public byte[] Value { get; set; }
    public string PersonalPhotoHash { get;set; }
}