using MandrilAPI.Models;

namespace MandrilAPI.Services;

public class MandrilDataStore
{
    public List<Mandril> Mandriles { get; set; }

    public static MandrilDataStore Current { get; } = new MandrilDataStore();

    public MandrilDataStore()
    {
        Mandriles = new List<Mandril>()
        {
            
        };
    }
}
