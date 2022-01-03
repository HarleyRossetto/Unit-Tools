using Macquarie.Handbook.Data.Transcript.Facts;

namespace Macquarie.Handbook.Data.Transcript;

public enum EnumGrade
{
    Fail = 0,
    Pass = 50,
    Credit = 65,
    Distinction = 75,
    HighDistinction = 85
}

//TODO Decide if the below code should be expanded/continued.
public record UnitGrade 
{
    public UnitGrade(EnumGrade? grade) => _grade = grade;

    private EnumGrade? _grade;
    public EnumGrade? Grade {
        get => _grade;
    
    }

    public static UnitGrade Parse(string s) {
        return s switch {
            "P" or "p"      => new UnitGrade(EnumGrade.Pass),
            "CR" or "cr"    => new UnitGrade(EnumGrade.Credit),
            "D" or "d"      => new UnitGrade(EnumGrade.Distinction),
            "HD" or "hd"    => new UnitGrade(EnumGrade.HighDistinction),
            _               => new UnitGrade(EnumGrade.Fail)
        };
    }
}
