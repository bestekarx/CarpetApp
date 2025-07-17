using CarpetApp.Enums;
using CarpetApp.Models.MessageTaskModels;
using CarpetApp.Resources.Strings;

namespace CarpetApp.Helpers;

public static class Consts
{
  public const string LoginPageParameter = nameof(LoginPageParameter);
  public const string DefaultLanguageCode = "tr-TR";
  public const string LanguageCode = nameof(LanguageCode);
  public const string Type = nameof(Type);

  #region Akavache Key

  public const string UserData = nameof(UserData);

  #endregion

  #region ParameterKey

  public const string ProductModel = nameof(ProductModel);
  public const string VehicleModel = nameof(VehicleModel);
  public const string AreaModel = nameof(AreaModel);
  public const string CompanyModel = nameof(CompanyModel);
  public const string SmsUsersModel = nameof(SmsUsersModel);
  public const string SmsTemplateModel = nameof(SmsTemplateModel);
  public const string SmsConfigurationModel = nameof(SmsConfigurationModel);

  #endregion

  #region ParamModelKey

  public const string State = nameof(State);
  public const string ProductType = nameof(ProductType);

  #endregion

  #region Pages Route

  public const string LoadingPage = "LoadingPage";
  public const string HomePage = "HomePage";
  public const string LoginPage = "Login";
  public const string DefinitionsPage = "Definitions";
  public const string ProductsPage = "Products";
  public const string ProductDetail = "ProductDetail";
  public const string FilterPage = "FilterPage";
  public const string VehiclesPage = "Vehicles";
  public const string VehicleDetail = "VehicleDetail";
  public const string AreasPage = "Areas";
  public const string AreaDetail = "AreaDetail";
  public const string CompaniesPage = "Companies";
  public const string CompanyDetail = "CompanyDetail";
  public const string SmsUsersPage = "SmsUsers";

  public const string SmsUserDetail = "SmsUserDetail";
  public const string ReceivedListPage = "ReceivedListPage";

  //public const string SmsTemplatesPage = "SmsTemplates";
  //public const string SmsTemplateDetail = "SmsTemplateDetail";
  public const string DataPage = "DataPage";
  public const string SmsConfigurationsPage = "SmsConfigurations";
  public const string SmsConfigurationDetail = "SmsConfigurationDetail";

  public const string TaskEditPopup = nameof(TaskEditPopup);

  #endregion

  #region Message Task Constants

  /// <summary>
  /// Message Task Type listesi
  /// </summary>
  public static List<MessageTaskTypeModel> MessageTaskTypeList => new()
  {
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.ReceivedCreated,
      TaskTypeName = AppStrings.ReceivedCreated,
      TaskTypeDescription = AppStrings.ReceivedCreatedDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.ReceivedCancelled,
      TaskTypeName = AppStrings.ReceivedCancelled,
      TaskTypeDescription = AppStrings.ReceivedCancelledDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.OrderCreated,
      TaskTypeName = AppStrings.OrderCreated,
      TaskTypeDescription = AppStrings.OrderCreatedDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.OrderCompleted,
      TaskTypeName = AppStrings.OrderCompleted,
      TaskTypeDescription = AppStrings.OrderCompletedDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.OrderCancelled,
      TaskTypeName = AppStrings.OrderCancelled,
      TaskTypeDescription = AppStrings.OrderCancelledDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.InvoiceCreated,
      TaskTypeName = AppStrings.InvoiceCreated,
      TaskTypeDescription = AppStrings.InvoiceCreatedDescription
    },
    new MessageTaskTypeModel
    {
      TaskType = MessageTaskType.InvoicePaid,
      TaskTypeName = AppStrings.InvoicePaid,
      TaskTypeDescription = AppStrings.InvoicePaidDescription
    }
  };

  /// <summary>
  /// Message Behaviour listesi
  /// </summary>
  public static List<MessageBehaviourModel> MessageBehaviourList => new()
  {
    new MessageBehaviourModel()
    {
      Behaviour = MessageBehavior.AlwaysSend,
      BehaviourName = AppStrings.BehaviourAlwaysSend,
    },
    new MessageBehaviourModel()
    {
      Behaviour = MessageBehavior.AskBeforeSend,
      BehaviourName = AppStrings.BehaviourAskBeforeSend,
    },
    new MessageBehaviourModel()
    {
      Behaviour = MessageBehavior.NeverSend,
      BehaviourName = AppStrings.BehaviourNeverSend,
    },
  };

  /// <summary>
  /// Task Type'a göre placeholder button'ları
  /// </summary>
  public static Dictionary<MessageTaskType, List<PlaceholderButtonModel>> TaskTypePlaceholders => new()
  {
    { MessageTaskType.ReceivedCreated, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagIsim, PlaceholderText = "{isim}" },
            new() { DisplayText = AppStrings.TagTarih, PlaceholderText = "{tarih}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" },
            new() { DisplayText = AppStrings.TagFirmaTelefonu, PlaceholderText = "{companyPhone}" }
        }
    },
    { MessageTaskType.ReceivedCancelled, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagIsim, PlaceholderText = "{isim}" },
            new() { DisplayText = AppStrings.TagIptalNedeni, PlaceholderText = "{iptalNedeni}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" }
        }
    },
    { MessageTaskType.OrderCreated, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagIsim, PlaceholderText = "{isim}" },
            new() { DisplayText = AppStrings.TagSiparisNo, PlaceholderText = "{siparisNo}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" },
            new() { DisplayText = AppStrings.TagFirmaTelefonu, PlaceholderText = "{companyPhone}" }
        }
    },
    { MessageTaskType.OrderCompleted, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagIsim, PlaceholderText = "{isim}" },
            new() { DisplayText = AppStrings.TagSiparisNo, PlaceholderText = "{siparisNo}" },
            new() { DisplayText = AppStrings.TagAdet, PlaceholderText = "{siparisAdet}" },
            new() { DisplayText = AppStrings.TagTutar, PlaceholderText = "{tutar}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" }
        }
    },
    { MessageTaskType.OrderCancelled, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagIsim, PlaceholderText = "{isim}" },
            new() { DisplayText = AppStrings.TagSiparisNo, PlaceholderText = "{siparisNo}" },
            new() { DisplayText = AppStrings.TagIptalNedeni, PlaceholderText = "{iptalNedeni}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" }
        }
    },
    { MessageTaskType.InvoiceCreated, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagFaturaNo, PlaceholderText = "{faturaNo}" },
            new() { DisplayText = AppStrings.TagTutar, PlaceholderText = "{tutar}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" }
        }
    },
    { MessageTaskType.InvoicePaid, new List<PlaceholderButtonModel>
        {
            new() { DisplayText = AppStrings.TagFaturaNo, PlaceholderText = "{faturaNo}" },
            new() { DisplayText = AppStrings.TagTutar, PlaceholderText = "{tutar}" },
            new() { DisplayText = AppStrings.TagFirmaAdi, PlaceholderText = "{companyName}" }
        }
    }
  };

  /// <summary>
  /// Task Type'a göre örnek şablonları
  /// </summary>
  public static Dictionary<MessageTaskType, string> TaskTypeSampleTemplates => new()
  {
    { MessageTaskType.ReceivedCreated, AppStrings.ReceivedCreatedSample },
    { MessageTaskType.ReceivedCancelled, AppStrings.ReceivedCancelledSample },
    { MessageTaskType.OrderCreated, AppStrings.OrderCreatedSample },
    { MessageTaskType.OrderCompleted, AppStrings.OrderCompletedSample },
    { MessageTaskType.OrderCancelled, AppStrings.OrderCancelledSample },
    { MessageTaskType.InvoiceCreated, AppStrings.InvoiceCreatedSample },
    { MessageTaskType.InvoicePaid, AppStrings.InvoicePaidSample }
  };

  #endregion

  #region SMS Placeholder Mappings

  /// <summary>
  /// SMS şablonunu PlaceholderMappings ve values kullanarak formatlar
  /// </summary>
  public static string FormatSmsMessage(string template, Dictionary<string, string> placeholderMappings, Dictionary<string, object> values)
  {
    if (string.IsNullOrEmpty(template) || placeholderMappings == null || values == null)
      return template;

    var formattedMessage = template;
    foreach (var mapping in placeholderMappings)
    {
      if (values.ContainsKey(mapping.Value))
      {
        formattedMessage = formattedMessage.Replace(mapping.Key, values[mapping.Value]?.ToString() ?? "");
      }
    }
    return formattedMessage;
  }

  /// <summary>
  /// Çok dilli Placeholder tanımları - Dinamik yapı
  /// </summary>
  private static readonly Dictionary<MessageTaskType, Dictionary<string, Dictionary<string, string>>> MultiLanguagePlaceholderMappings = new()
  {
    {
      MessageTaskType.ReceivedCreated, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{isim}", "FullName" },
            { "{tarih}", "PickupDate" },
            { "{companyName}", "Name" },
            { "{companyPhone}", "Description" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{date}", "ReceivedDate" },
            { "{companyName}", "CompanyName" },
            { "{companyPhone}", "CompanyPhone" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{datum}", "ReceivedDate" },
            { "{firmaName}", "CompanyName" },
            { "{firmaTelefon}", "CompanyPhone" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{nom}", "CustomerName" },
            { "{date}", "ReceivedDate" },
            { "{nomEntreprise}", "CompanyName" },
            { "{telephoneEntreprise}", "CompanyPhone" }
          }
        }
      }
    },
    {
      MessageTaskType.ReceivedCancelled, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{isim}", "FullName" },
            { "{iptalNedeni}", "CancellationReason" },
            { "{companyName}", "Name" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{cancellationReason}", "CancellationReason" },
            { "{companyName}", "CompanyName" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{stornierungsgrund}", "CancellationReason" },
            { "{firmaName}", "CompanyName" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{nom}", "CustomerName" },
            { "{raisonAnnulation}", "CancellationReason" },
            { "{nomEntreprise}", "CompanyName" }
          }
        }
      }
    },
    {
      MessageTaskType.OrderCreated, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{isim}", "FullName" },
            { "{siparisNo}", "FicheNo" }, // Fiş No = Received.FicheNo
            { "{companyName}", "Name" },
            { "{companyPhone}", "Description" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{orderNumber}", "OrderNumber" },
            { "{companyName}", "CompanyName" },
            { "{companyPhone}", "CompanyPhone" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{bestellnummer}", "OrderNumber" },
            { "{firmaName}", "CompanyName" },
            { "{firmaTelefon}", "CompanyPhone" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{nom}", "CustomerName" },
            { "{numeroCommande}", "OrderNumber" },
            { "{nomEntreprise}", "CompanyName" },
            { "{telephoneEntreprise}", "CompanyPhone" }
          }
        }
      }
    },
    {
      MessageTaskType.OrderCompleted, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{isim}", "FullName" },
            { "{siparisNo}", "FicheNo" }, // Fiş No = Received.FicheNo
            { "{siparisAdet}", "Number" },
            { "{tutar}", "OrderTotalPrice" },
            { "{companyName}", "Name" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{orderNumber}", "OrderNumber" },
            { "{quantity}", "OrderQuantity" },
            { "{amount}", "OrderAmount" },
            { "{companyName}", "CompanyName" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{bestellnummer}", "OrderNumber" },
            { "{menge}", "OrderQuantity" },
            { "{betrag}", "OrderAmount" },
            { "{firmaName}", "CompanyName" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{nom}", "CustomerName" },
            { "{numeroCommande}", "OrderNumber" },
            { "{quantite}", "OrderQuantity" },
            { "{montant}", "OrderAmount" },
            { "{nomEntreprise}", "CompanyName" }
          }
        }
      }
    },
    {
      MessageTaskType.OrderCancelled, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{isim}", "FullName" },
            { "{siparisNo}", "FicheNo" }, // Fiş No = Received.FicheNo
            { "{iptalNedeni}", "CancellationReason" },
            { "{companyName}", "Name" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{orderNumber}", "OrderNumber" },
            { "{cancellationReason}", "CancellationReason" },
            { "{companyName}", "CompanyName" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{name}", "CustomerName" },
            { "{bestellnummer}", "OrderNumber" },
            { "{stornierungsgrund}", "CancellationReason" },
            { "{firmaName}", "CompanyName" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{nom}", "CustomerName" },
            { "{numeroCommande}", "OrderNumber" },
            { "{raisonAnnulation}", "CancellationReason" },
            { "{nomEntreprise}", "CompanyName" }
          }
        }
      }
    },
    {
      MessageTaskType.InvoiceCreated, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{faturaNo}", "FicheNo" }, // Fiş No = Received.FicheNo
            { "{tutar}", "TotalPrice" },
            { "{companyName}", "Name" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{invoiceNumber}", "InvoiceNumber" },
            { "{amount}", "InvoiceAmount" },
            { "{companyName}", "CompanyName" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{rechnungsnummer}", "InvoiceNumber" },
            { "{betrag}", "InvoiceAmount" },
            { "{firmaName}", "CompanyName" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{numeroFacture}", "InvoiceNumber" },
            { "{montant}", "InvoiceAmount" },
            { "{nomEntreprise}", "CompanyName" }
          }
        }
      }
    },
    {
      MessageTaskType.InvoicePaid, new Dictionary<string, Dictionary<string, string>>
      {
        {
          "tr-TR", new Dictionary<string, string>
          {
            { "{faturaNo}", "FicheNo" }, // Fiş No = Received.FicheNo
            { "{tutar}", "TotalPrice" },
            { "{companyName}", "Name" }
          }
        },
        {
          "en-US", new Dictionary<string, string>
          {
            { "{invoiceNumber}", "InvoiceNumber" },
            { "{amount}", "InvoiceAmount" },
            { "{companyName}", "CompanyName" }
          }
        },
        {
          "de-DE", new Dictionary<string, string>
          {
            { "{rechnungsnummer}", "InvoiceNumber" },
            { "{betrag}", "InvoiceAmount" },
            { "{firmaName}", "CompanyName" }
          }
        },
        {
          "fr-FR", new Dictionary<string, string>
          {
            { "{numeroFacture}", "InvoiceNumber" },
            { "{montant}", "InvoiceAmount" },
            { "{nomEntreprise}", "CompanyName" }
          }
        }
      }
    }
  };

  /// <summary>
  /// TaskType ve kültür koduna göre PlaceholderMappings döndürür
  /// Dinamik ve genişletilebilir yapı
  /// </summary>
  public static Dictionary<string, string> GetPlaceholderMappingsForTaskType(MessageTaskType taskType, string cultureCode = "tr-TR")
  {
    // Desteklenen kültürler listesi
    var supportedCultures = new[] { "tr-TR", "en-US", "de-DE", "fr-FR" };
    
    // Eğer culture desteklenmiyorsa, en yakın olanı bul veya default'a dön
    var normalizedCulture = supportedCultures.FirstOrDefault(c => c.StartsWith(cultureCode.Split('-')[0])) ?? "tr-TR";
    
    if (MultiLanguagePlaceholderMappings.TryGetValue(taskType, out var taskMappings))
    {
      if (taskMappings.TryGetValue(normalizedCulture, out var cultureMappings))
      {
        return new Dictionary<string, string>(cultureMappings);
      }
      
      // Fallback: İlk mevcut kültürü kullan
      return new Dictionary<string, string>(taskMappings.Values.First());
    }
    
    return new Dictionary<string, string>();
  }

  /// <summary>
  /// Desteklenen kültürleri döndürür
  /// </summary>
  public static List<string> GetSupportedCultures()
  {
    return new List<string> { "tr-TR", "en-US", "de-DE", "fr-FR" };
  }

  /// <summary>
  /// Belirli bir TaskType için desteklenen placeholder'ları kültüre göre döndürür
  /// </summary>
  public static List<PlaceholderButtonModel> GetPlaceholdersForTaskType(MessageTaskType taskType, string cultureCode = "tr-TR")
  {
    var mappings = GetPlaceholderMappingsForTaskType(taskType, cultureCode);
    return mappings.Select(m => new PlaceholderButtonModel 
    { 
      PlaceholderText = m.Key, 
      DisplayText = GetDisplayTextForPlaceholder(m.Key, cultureCode)
    }).ToList();
  }

  /// <summary>
  /// Placeholder için görüntülenecek metni döndürür
  /// </summary>
  private static string GetDisplayTextForPlaceholder(string placeholder, string cultureCode)
  {
    var displayMappings = new Dictionary<string, Dictionary<string, string>>
    {
      {
        "tr-TR", new Dictionary<string, string>
        {
          { "{isim}", "İsim" },
          { "{tarih}", "Tarih" },
          { "{siparisNo}", "Sipariş No" },
          { "{siparisAdet}", "Adet" },
          { "{tutar}", "Tutar" },
          { "{faturaNo}", "Fatura No" },
          { "{iptalNedeni}", "İptal Nedeni" },
          { "{companyName}", "Firma Adı" },
          { "{companyPhone}", "Firma Telefonu" }
        }
      },
      {
        "en-US", new Dictionary<string, string>
        {
          { "{name}", "Name" },
          { "{date}", "Date" },
          { "{orderNumber}", "Order Number" },
          { "{quantity}", "Quantity" },
          { "{amount}", "Amount" },
          { "{invoiceNumber}", "Invoice Number" },
          { "{cancellationReason}", "Cancellation Reason" },
          { "{companyName}", "Company Name" },
          { "{companyPhone}", "Company Phone" }
        }
      },
      {
        "de-DE", new Dictionary<string, string>
        {
          { "{name}", "Name" },
          { "{datum}", "Datum" },
          { "{bestellnummer}", "Bestellnummer" },
          { "{menge}", "Menge" },
          { "{betrag}", "Betrag" },
          { "{rechnungsnummer}", "Rechnungsnummer" },
          { "{stornierungsgrund}", "Stornierungsgrund" },
          { "{firmaName}", "Firmenname" },
          { "{firmaTelefon}", "Firmentelefon" }
        }
      },
      {
        "fr-FR", new Dictionary<string, string>
        {
          { "{nom}", "Nom" },
          { "{date}", "Date" },
          { "{numeroCommande}", "Numéro de commande" },
          { "{quantite}", "Quantité" },
          { "{montant}", "Montant" },
          { "{numeroFacture}", "Numéro de facture" },
          { "{raisonAnnulation}", "Raison d'annulation" },
          { "{nomEntreprise}", "Nom de l'entreprise" },
          { "{telephoneEntreprise}", "Téléphone de l'entreprise" }
        }
      }
    };

    var supportedCultures = new[] { "tr-TR", "en-US", "de-DE", "fr-FR" };
    var normalizedCulture = supportedCultures.FirstOrDefault(c => c.StartsWith(cultureCode.Split('-')[0])) ?? "tr-TR";

    if (displayMappings.TryGetValue(normalizedCulture, out var cultureMappings) &&
        cultureMappings.TryGetValue(placeholder, out var displayText))
    {
      return displayText;
    }

    return placeholder; // Fallback to placeholder itself
  }

  #endregion
}

public class PlaceholderButtonModel
{
    public string DisplayText { get; set; }
    public string PlaceholderText { get; set; }
}