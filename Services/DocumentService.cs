using Microsoft.Extensions.Options;
using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using CompManager.Helpers;
using CompManager.Entities;
using CompManager.Models.Competences;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CompManager.Services
{
  public interface IDocumentService
  {
    String Create(CompetenceProfileCreateRequest model, Account account);
  }

  public class DocumentService : IDocumentService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public DocumentService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    private IEnumerable<Competence> GetCompetenceResponse(int[] requestedCompetences, int accountId)
    {
      IQueryable<Competence> competences = _context.Competences
      .Include(c => c.Process).Where(c => c.AccountId == accountId
      && requestedCompetences.Contains(c.Id));
      return competences;
    }

    public String Create(CompetenceProfileCreateRequest model, Account account)
    {
      Guid documentGuid = Guid.NewGuid();
      List<Competence> competences = GetCompetenceResponse(model.Competences, account.Id).ToList();

      using (WordprocessingDocument doc = WordprocessingDocument.Create(String.Format("./docs/{0}.docx", documentGuid), WordprocessingDocumentType.Document))
      {
        //// Creates the MainDocumentPart and add it to the document (doc)
        MainDocumentPart mainPart = doc.AddMainDocumentPart();
        mainPart.Document = new Document();
        Body body = new Body();

        body.Append(TitleParagraph(account.FirstName, account.LastName));
        body.Append(EmptyParagraph());
        body.Append(Section());
        foreach (Competence competence in competences)
        {
          body.Append(ContentTitle(competence.Process.Name));
          body.Append(Content(competence.Action));
        }

        body.Append(SectionProperties());
        mainPart.Document.Append(body);

      }

      String file = Convert.ToBase64String(File.ReadAllBytes(String.Format("./docs/{0}.docx", documentGuid)));
      Console.WriteLine(file.Length);

      return file;
    }

    public Paragraph TitleParagraph(string firstName, string lastName)
    {
      Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "00EE6936", RsidParagraphProperties = "005E2700", RsidRunAdditionDefault = "005E2700", ParagraphId = "28B1BD59", TextId = "11E04188" };
      ParagraphProperties paragraphProperties1 = new ParagraphProperties();
      ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "Titel" };
      Justification justification1 = new Justification() { Val = JustificationValues.Center };

      ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
      RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      FontSize fontSize1 = new FontSize() { Val = "48" };
      FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "48" };
      Languages languages1 = new Languages() { Val = "de-DE" };

      paragraphMarkRunProperties1.Append(runFonts1);
      paragraphMarkRunProperties1.Append(fontSize1);
      paragraphMarkRunProperties1.Append(fontSizeComplexScript1);
      paragraphMarkRunProperties1.Append(languages1);

      paragraphProperties1.Append(paragraphStyleId1);
      paragraphProperties1.Append(justification1);
      paragraphProperties1.Append(paragraphMarkRunProperties1);
      ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.SpellStart };

      Run run1 = new Run() { RsidRunProperties = "005E2700" };

      RunProperties runProperties1 = new RunProperties();
      RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      FontSize fontSize2 = new FontSize() { Val = "48" };
      FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = "48" };
      Languages languages2 = new Languages() { Val = "de-DE" };

      runProperties1.Append(runFonts2);
      runProperties1.Append(fontSize2);
      runProperties1.Append(fontSizeComplexScript2);
      runProperties1.Append(languages2);
      Text text1 = new Text();
      text1.Text = String.Format("Kompetenzprofil {0} {1}", firstName, lastName);

      run1.Append(runProperties1);
      run1.Append(text1);
      paragraph1.Append(paragraphProperties1);
      paragraph1.Append(run1);

      return paragraph1;
    }

    private Paragraph EmptyParagraph()
    {
      Paragraph paragraph2 = new Paragraph() { RsidParagraphAddition = "005E2700", RsidParagraphProperties = "005E2700", RsidRunAdditionDefault = "005E2700", ParagraphId = "633EB24B", TextId = "08844DAA" };
      ParagraphProperties paragraphProperties2 = new ParagraphProperties();
      ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
      Languages languages4 = new Languages() { Val = "de-DE" };
      paragraphMarkRunProperties2.Append(languages4);
      paragraphProperties2.Append(paragraphMarkRunProperties2);
      paragraph2.Append(paragraphProperties2);
      return paragraph2;
    }

    private Paragraph Section()
    {
      Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "005E2700", RsidParagraphProperties = "005E2700", RsidRunAdditionDefault = "005E2700", ParagraphId = "78AF6870", TextId = "77777777" };
      ParagraphProperties paragraphProperties3 = new ParagraphProperties();
      ParagraphMarkRunProperties paragraphMarkRunProperties3 = new ParagraphMarkRunProperties();
      Languages languages5 = new Languages() { Val = "de-DE" };
      paragraphMarkRunProperties3.Append(languages5);
      SectionProperties sectionProperties1 = new SectionProperties() { RsidR = "005E2700" };
      PageSize pageSize1 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
      PageMargin pageMargin1 = new PageMargin() { Top = 1440, Right = (UInt32Value)1440U, Bottom = 1440, Left = (UInt32Value)1440U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
      Columns columns1 = new Columns() { Space = "708" };
      DocGrid docGrid1 = new DocGrid() { LinePitch = 360 };
      sectionProperties1.Append(pageSize1);
      sectionProperties1.Append(pageMargin1);
      sectionProperties1.Append(columns1);
      sectionProperties1.Append(docGrid1);
      paragraphProperties3.Append(paragraphMarkRunProperties3);
      paragraphProperties3.Append(sectionProperties1);
      paragraph3.Append(paragraphProperties3);

      return paragraph3;
    }

    private Paragraph ContentTitle(String title)
    {
      Paragraph paragraph1 = new Paragraph() { RsidParagraphMarkRevision = "005E2700", RsidParagraphAddition = "005E2700", RsidParagraphProperties = "00114E01", RsidRunAdditionDefault = "005E2700", ParagraphId = "56A42B67", TextId = "75EF4C40" };
      ParagraphProperties paragraphProperties1 = new ParagraphProperties();
      SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };
      ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
      RunFonts runFonts1 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      Bold bold1 = new Bold();
      BoldComplexScript boldComplexScript1 = new BoldComplexScript();
      Languages languages1 = new Languages() { Val = "de-DE" };
      paragraphMarkRunProperties1.Append(runFonts1);
      paragraphMarkRunProperties1.Append(bold1);
      paragraphMarkRunProperties1.Append(boldComplexScript1);
      paragraphMarkRunProperties1.Append(languages1);
      paragraphProperties1.Append(spacingBetweenLines1);
      paragraphProperties1.Append(paragraphMarkRunProperties1);
      ProofError proofError1 = new ProofError() { Type = ProofingErrorValues.SpellStart };
      Run run1 = new Run() { RsidRunProperties = "005E2700" };
      RunProperties runProperties1 = new RunProperties();
      RunFonts runFonts2 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      Bold bold2 = new Bold();
      BoldComplexScript boldComplexScript2 = new BoldComplexScript();
      Languages languages2 = new Languages() { Val = "de-DE" };
      runProperties1.Append(runFonts2);
      runProperties1.Append(bold2);
      runProperties1.Append(boldComplexScript2);
      runProperties1.Append(languages2);
      Text text1 = new Text();
      text1.Text = title;
      run1.Append(runProperties1);
      run1.Append(text1);
      ProofError proofError2 = new ProofError() { Type = ProofingErrorValues.SpellEnd };
      paragraph1.Append(paragraphProperties1);
      paragraph1.Append(proofError1);
      paragraph1.Append(run1);
      paragraph1.Append(proofError2);
      return paragraph1;
    }

    private Paragraph Content(String content)
    {
      Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "005E2700", RsidParagraphAddition = "005E2700", RsidParagraphProperties = "005E2700", RsidRunAdditionDefault = "005E2700", ParagraphId = "784D36AC", TextId = "017233B8" };
      ParagraphProperties paragraphProperties5 = new ParagraphProperties();
      ParagraphMarkRunProperties paragraphMarkRunProperties5 = new ParagraphMarkRunProperties();
      RunFonts runFonts6 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      Languages languages8 = new Languages() { Val = "de-DE" };
      paragraphMarkRunProperties5.Append(runFonts6);
      paragraphMarkRunProperties5.Append(languages8);
      paragraphProperties5.Append(paragraphMarkRunProperties5);
      Run run4 = new Run() { RsidRunProperties = "005E2700" };
      RunProperties runProperties4 = new RunProperties();
      RunFonts runFonts7 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
      Languages languages9 = new Languages() { Val = "de-DE" };
      runProperties4.Append(runFonts7);
      runProperties4.Append(languages9);
      Text text4 = new Text();
      text4.Text = content;
      run4.Append(runProperties4);
      run4.Append(text4);
      paragraph5.Append(paragraphProperties5);
      paragraph5.Append(run4);

      return paragraph5;
    }

    private SectionProperties SectionProperties()
    {
      SectionProperties sectionProperties2 = new SectionProperties() { RsidRPr = "005E2700", RsidR = "005E2700", RsidSect = "005E2700" };
      SectionType sectionType1 = new SectionType() { Val = SectionMarkValues.Continuous };
      PageSize pageSize2 = new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U };
      PageMargin pageMargin2 = new PageMargin() { Top = 1440, Right = (UInt32Value)1440U, Bottom = 1440, Left = (UInt32Value)1440U, Header = (UInt32Value)708U, Footer = (UInt32Value)708U, Gutter = (UInt32Value)0U };
      Columns columns2 = new Columns() { Space = "708", ColumnCount = 2 };
      DocGrid docGrid2 = new DocGrid() { LinePitch = 360 };

      sectionProperties2.Append(sectionType1);
      sectionProperties2.Append(pageSize2);
      sectionProperties2.Append(pageMargin2);
      sectionProperties2.Append(columns2);
      sectionProperties2.Append(docGrid2);
      return sectionProperties2;
    }
  }

}