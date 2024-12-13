using System.Collections.Generic;
using System.IO; // For file creation
using System.Net;
using System.Net.Mail;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EmailSender : MonoBehaviour
{
    public InputField recipientEmailInput;
    public InventoryManager inventoryManager;
    public Text titleText;

    public ItemList temporaryList;
    // List for Unity Inspector to assign photos
    public List<Sprite> photoSprites;

    public void SendEmailWithAttachment()
    {
        // Sender's email and SMTP configuration
        string fromEmail = "AppCompras2024@gmail.com";
        string smtpServer = "smtp.gmail.com";
        int smtpPort = 587;
        string smtpUsername = "AppCompras2024@gmail.com";
        string smtpPassword = "capk dxan qzge gzxa";

        // Retrieve recipient's email from the input field
        string toEmail = recipientEmailInput.text;
        if (string.IsNullOrEmpty(toEmail))
        {
            Debug.LogError("Recipient email is empty. Please enter a valid email.");
            return;
        }

        // Email subject and body
        string subject = "Tu Lista de Compras está Lista!";
        string body = "Hola! Aquí tienes tu lista de compras adjunta como archivo de texto.";

        // Generate the .txt file
        string filePath = GenerateShoppingListHtml();

        try
        {
            // Create the email message
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;

            // Attach the .txt file
            if (!string.IsNullOrEmpty(filePath))
            {
                Attachment attachment = new Attachment(filePath);
                mail.Attachments.Add(attachment);
            }

            // Configure the SMTP client
            SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
            smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);
            Debug.Log("Email with attachment sent successfully!");

            // Optionally delete the file after sending
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Temporary file deleted.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to send email: " + ex.Message);
        }
    }

    private string GenerateShoppingListHtml()
    {
        if (temporaryList == null || temporaryList.items.Count == 0)
        {
            Debug.LogError("Temporary list is empty or not found. Unable to generate the file.");
            return null;
        }


        string filePath = Path.Combine(Application.persistentDataPath, "ShoppingList.html");
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<!DOCTYPE html>");
                writer.WriteLine("<html lang='en'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<meta charset='UTF-8'>");
                writer.WriteLine("<title>Lista de Compras</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("body { font-family: Arial, sans-serif; background-color: #fff8dc; padding: 20px; }");
                writer.WriteLine("h1 { color: #ff6347; }");
                writer.WriteLine(".item { margin: 5px 0; }");
                writer.WriteLine(".total { font-weight: bold; color: #008000; margin-top: 20px; }");
                writer.WriteLine(".photos { display: flex; flex-wrap: wrap; gap: 10px; margin-top: 20px; }");
                writer.WriteLine(".photos img { width: 100px; height: 100px; object-fit: cover; border: 1px solid #ccc; }");
                writer.WriteLine("</style>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine("<h1>Lista de Compras</h1>");
                writer.WriteLine("<hr>");
                writer.WriteLine("<ul>");

                float totalPrice = 0;
                foreach (var item in temporaryList.items)
                {
                    writer.WriteLine($"<li class='item'>• <strong>{item.itemName}</strong>: {item.price}$</li>");
                    totalPrice += item.price;
                }

                writer.WriteLine("</ul>");
                writer.WriteLine($"<div class='total'>Total: {totalPrice}$</div>");
                writer.WriteLine("<p><strong>Encuéntralos en tus supermercados</strong></p>");

                writer.WriteLine("<div class='photos'>");

                // Loop through photoSprites and convert them to Base64 for embedding in HTML
                foreach (var sprite in photoSprites)
                {
                    Texture2D texture = sprite.texture;
                    byte[] imageData = texture.EncodeToPNG(); // Encode texture to PNG
                    string base64Image = System.Convert.ToBase64String(imageData);
                    writer.WriteLine($"<img src='data:image/png;base64,{base64Image}' alt='Producto' />");
                }

                writer.WriteLine("</div>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }

            Debug.Log($"HTML file generated successfully at: {filePath}");
            return filePath;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to generate the HTML file: " + ex.Message);
            return null;
        }
    }



    private string GenerateShoppingListTxt()
    {
        if (temporaryList == null || temporaryList.items.Count == 0)
        {
            Debug.LogError("Temporary list is empty or not found. Unable to generate the file.");
            return null;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "ShoppingList.txt");
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Lista de Compras:");
                writer.WriteLine("=================");
                float totalPrice = 0;

                foreach (var item in temporaryList.items)
                {
                    writer.WriteLine($"• {item.itemName}: {item.price}$");
                    totalPrice += item.price;
                }

                writer.WriteLine();
                writer.WriteLine($"Total: {totalPrice}$");
            }

            Debug.Log($"File generated successfully at: {filePath}");
            return filePath;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to generate the file: " + ex.Message);
            return null;
        }
    }

    public void findList()
    {
        temporaryList = inventoryManager.itemListsShareable.Find(list => list.listName == titleText.text);

        if (temporaryList != null)
        {
            temporaryList = new ItemList(temporaryList.listName)
            {
                items = new List<Item>(temporaryList.items.Select(item => new Item(
                    item.itemName,
                    item.itemDescription,
                    item.price,
                    item.imagePath
                )))
            };

            Debug.Log("Temporary List Items:");
            foreach (var item in temporaryList.items)
            {
                Debug.Log($"Name: {item.itemName}, Description: {item.itemDescription}, Price: {item.price}, ImagePath: {item.imagePath}");
            }
        }
        else
        {
            Debug.LogWarning("No list found with the specified name.");
        }
    }
}
