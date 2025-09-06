using WorkFound.Domain.Entities.Enums;

namespace WorkFound.Application.Validation;

public class JobPostValidator
{
    private static readonly Dictionary<string, string[]> ValidRegions = new()
    {
        // Middle East and North Africa
        { "MENA", ["AE", "SA", "EG", "JO", "QA", "KW", "OM", "BH", "DZ", "MA", "TN", "LB", "LY", "SD"] },
        // Europe
        { "EU", ["DE", "FR", "GB", "IT", "ES", "NL", "BE", "PL", "SE", "NO", "FI", "DK", "PT", "IE", "AT", "CH", "CZ", "HU", "RO", "BG", "GR"
            ]
        },
        // North America
        { "NA", ["US", "CA", "MX"] },
        // Latin America & Caribbean
        { "LATAM", ["BR", "AR", "CL", "CO", "PE", "UY", "EC", "VE", "DO", "CR", "GT", "HN", "NI", "PA", "PR"] },
        // Asia-Pacific
        { "APAC", ["CN", "JP", "KR", "IN", "SG", "MY", "TH", "VN", "PH", "AU", "NZ", "ID", "PK", "BD"] },
        // Africa (Sub-Saharan)
        { "AFRICA", ["NG", "KE", "GH", "ZA", "TZ", "UG", "ET", "CM", "SN", "ZM", "ZW", "MZ"] },
        // Global (catch-all)
        { "ANYWHERE", [] }
    };
    
    private static readonly HashSet<string> ValidCountryCodes = ValidRegions.SelectMany(kvp => kvp.Value).ToHashSet();

    public static ValidationResult ValidateLocation(JobLocationType locationType, string location)
    {
        switch (locationType)
        {
            case JobLocationType.OnSite:
            case JobLocationType.Hybrid:
            case JobLocationType.RemoteCountry:
                if (!ValidCountryCodes.Contains(location.ToUpper()))
                    return new ValidationResult(false, $"Invalid country code: {location}");
                break;
            
            case JobLocationType.RemoteRegion:
                if (!ValidRegions.ContainsKey(location.ToUpper()))
                    return new ValidationResult(false, $"Invalid region: {location}");
                break;
        }
        return new ValidationResult(true);
    }
}