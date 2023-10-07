﻿namespace getQuote.Framework
{
    public enum DocumentTypes : int
    {
        CPF,
        RG,
        CNPJ
    }

    public enum SelectDefault
    {
        None
    }

    public enum ProposalIncludes
    {
        None,
        Person,
        PersonContact,
        ProposalHistory,
        Item,
        ItemImageList
    }

    public enum PersonIncludes
    {
        None,
        Contact,
        Document
    }

    public enum ItemIncludes
    {
        None,
        ItemImage
    }

    public enum ProposalHistoryIncludes
    {
        None,
        Person,
        Proposal
    }

    public enum CompanyIncludes
    {
        None
    }

    public enum LoginLogStatus
    {
        None,
        Failed,
        Success
    }
}
