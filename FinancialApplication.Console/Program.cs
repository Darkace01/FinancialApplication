// See https://aka.ms/new-console-template for more information
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text.Json;

Console.WriteLine("Hello, World!");

Cloudinary cloudinary = new Cloudinary("cloudinary://996252828394623:ENn35fsH4TO5EAu2K7qiE5ScWV0@dklpwuijd");
cloudinary.Api.Secure = true;

var uploadParams = new ImageUploadParams()
{
    File = new FileDescription(@"https://miro.medium.com/v2/1*-Y9ozbNWSViiCmal1TT32w.jpeg"),
    UseFilename = true,
    UniqueFilename = true,
    Overwrite = true
};
var uploadResult = cloudinary.Upload(uploadParams);
Console.WriteLine(uploadResult.JsonObj);

// Get details of the image and run quality analysis

Console.WriteLine(uploadResult.SecureUri?.AbsoluteUri);
Console.WriteLine("Hello, World!");
Console.WriteLine(uploadResult);
Console.WriteLine(uploadResult.PublicId);

Console.WriteLine("Hello, World!");
//==============================

var uploadStr = JsonSerializer.Serialize(uploadResult);

Console.WriteLine(uploadStr);

Console.WriteLine("===============================");

var getResourceParams = new GetResourceParams("0_cCw2LfDjPq2kZynq")
{
    QualityAnalysis = true
};
var getResourceResult = cloudinary.GetResource(getResourceParams);
var resultJson = getResourceResult.JsonObj;

// Log quality analysis score to the console
Console.WriteLine(resultJson["quality_analysis"]);


// Transform the uploaded asset and generate a URL and image tag
//==============================

var myTransformation = cloudinary.Api.UrlImgUp.Transform(new Transformation()
    .Width(100).Crop("scale").Chain()
    .Effect(""));

var myUrl = myTransformation.BuildUrl("0_cCw2LfDjPq2kZynq");
var myImageTag = myTransformation.BuildImageTag("0_cCw2LfDjPq2kZynq");

// Log the URL of the transformed asset to the console
Console.WriteLine(myUrl);

// Log the image tag for the transformed asset to the console
Console.WriteLine(myImageTag);

Console.ReadLine();