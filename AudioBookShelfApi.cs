public class AbsLibrary
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string MediaType { get; set; }
}

public class AbsGetLibrariesResponse
{
    public List<AbsLibrary> Libraries { get; set; }
}

public class AbsGetSeriesResponse
{
    public List<AbsSeriesBooks> Results { get; set; }
}

public class AbsSeriesBooks
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<AbsLibraryItem> Books { get; set; }

}

public class AbsLibraryItem
{
    public string Id { get; set; }
    public string MediaType { get; set; }
    public AbsBook Media { get; set; }
    public string RelPath { get; set; }

}

public class AbsBook {
    public string LibraryItemId { get; set; }

    public AbsEBookFile EBookFile { get; set; }
    public AbsMetaData MetaData {get; set;}
}

public class AbsMetaData {
    public string Title { get; set; }
}

public class AbsEBookFile
{
    public string Ino { get; set; }
    public AbsFileMetaData Metadata { get; set; }
}

public class AbsFileMetaData {
    public string FileName { get; set; }
}

public class AbsBatchUpdateBook
{
    public string Id { get; set; }
    public AbsUpdateBookParameter MediaPayload { get; set; }
}

public class AbsUpdateBookParameter {
    public AbsUpdateBookMetaData Metadata { get; set; }
}

public class AbsUpdateBookMetaData {
    public List<AbsUpdateBookSeriesSequence> Series { get; set; }
}

public class AbsUpdateBookSeriesSequence {
    public string DisplayName { get; set; }
    public string Name { get; set; }
    public string Sequence { get; set; }
}