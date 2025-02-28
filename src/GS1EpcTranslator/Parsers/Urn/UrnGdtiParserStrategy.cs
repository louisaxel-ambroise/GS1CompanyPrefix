﻿namespace GS1EpcTranslator.Parsers.Urn;

/// <summary>
/// Implementation of <see cref="IEpcParserStrategy"/> that matches GDTI in URN format
/// </summary>
/// <param name="companyPrefixProvider">The GCP prefix provider</param>
public sealed class UrnGdtiParserStrategy(GS1CompanyPrefixProvider gcpProvider) : IEpcParserStrategy
{
    /// <summary>
    /// Matches the URN GDTI format
    /// </summary>
    public string Pattern => "^urn:epc:id:gdti:(?<gcp>\\d{6,12})\\.(?<documentType>\\d{0,6})(?<=[\\d\\.]{13}).(?<serial>.+)$";

    /// <summary>
    /// Transforms the URN GDTI parsed values into a <see cref="IEpcFormatter"/>
    /// </summary>
    /// <param name="values">The values retrieved from the regex match</param>
    /// <returns>The <see cref="IEpcFormatter"/> for the GDTI value</returns>
    public IEpcFormatter Transform(IDictionary<string, string> values)
    {
        var serial = values["serial"].ToGraphicSymbol();

        Alphanumeric.Validate(serial, 17);
        CompanyPrefixValidator.VerifyGcpLength(values["gcp"], gcpProvider);

        return new GdtiFormatter(
            gcp: values["gcp"],
            documentType: values["documentType"],
            serial: serial);
    }
}
