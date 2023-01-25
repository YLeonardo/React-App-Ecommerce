// Carlos Pineda Guerrero. 2021-2023
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class Get
    {
        [FunctionName("Get")]
        [ResponseCache(Duration = 86400, NoStore = false)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string path = (string)req.Query["nombre"];

                // el par�metro "descargar" es opcional, por default es "NO"
                bool descargar = ((string)req.Query["descargar"] ?? "NO").ToUpper() == "SI";
                // la variable de entorno HOME est� predefinida en el servidor (C:\home o D:\home)
                // Los archivos cargados se pueden ver en el portal, seleccionando la aplicaci�n de funciones y 
                // la opci�n "Consola" en la secci�n "Herramientas de desarrollo"
                string home = Environment.GetEnvironmentVariable("HOME");

                try
                {
                    byte[] contenido = File.ReadAllBytes(home + "/data" + path);
                    string nombre = Path.GetFileName(path);
                    string tipo_mime = MimeMapping.GetMimeMapping(path);
                    DateTime fecha_modificacion = File.GetLastWriteTime(home + "/data" + path);

                    if (descargar)
                        // el navegador descargar� el contenido como archivo
                        return new FileContentResult(contenido, tipo_mime) { FileDownloadName = nombre };
                    else
                        // el navegador mostrar� el contenido
                        // se envia al navegador el tipo mime para que lo procese adecuadamente
                        // se envia al navegador la fecha de modificaci�n del archivo para usar HTTP caching
                        return new FileContentResult(contenido, tipo_mime) { LastModified = fecha_modificacion };
                }
                catch (FileNotFoundException)
                {
                    return new NotFoundResult();
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }

    internal class MimeMapping
    {
        static Dictionary<string, string> mimeMap = new Dictionary<string, string>();
        static MimeMapping()
        {
            mimeMap.Add(".323", "text/h323");
            mimeMap.Add(".aaf", "application/octet-stream");
            mimeMap.Add(".aca", "application/octet-stream");
            mimeMap.Add(".accdb", "application/msaccess");
            mimeMap.Add(".accde", "application/msaccess");
            mimeMap.Add(".accdt", "application/msaccess");
            mimeMap.Add(".acx", "application/internet-property-stream");
            mimeMap.Add(".afm", "application/octet-stream");
            mimeMap.Add(".ai", "application/postscript");
            mimeMap.Add(".aif", "audio/x-aiff");
            mimeMap.Add(".aifc", "audio/aiff");
            mimeMap.Add(".aiff", "audio/aiff");
            mimeMap.Add(".application", "application/x-ms-application");
            mimeMap.Add(".art", "image/x-jg");
            mimeMap.Add(".asd", "application/octet-stream");
            mimeMap.Add(".asf", "video/x-ms-asf");
            mimeMap.Add(".asi", "application/octet-stream");
            mimeMap.Add(".asm", "text/plain");
            mimeMap.Add(".asr", "video/x-ms-asf");
            mimeMap.Add(".asx", "video/x-ms-asf");
            mimeMap.Add(".atom", "application/atom+xml");
            mimeMap.Add(".au", "audio/basic");
            mimeMap.Add(".avi", "video/x-msvideo");
            mimeMap.Add(".axs", "application/olescript");
            mimeMap.Add(".bas", "text/plain");
            mimeMap.Add(".bcpio", "application/x-bcpio");
            mimeMap.Add(".bin", "application/octet-stream");
            mimeMap.Add(".bmp", "image/bmp");
            mimeMap.Add(".c", "text/plain");
            mimeMap.Add(".cab", "application/octet-stream");
            mimeMap.Add(".calx", "application/vnd.ms-office.calx");
            mimeMap.Add(".cat", "application/vnd.ms-pki.seccat");
            mimeMap.Add(".cdf", "application/x-cdf");
            mimeMap.Add(".chm", "application/octet-stream");
            mimeMap.Add(".class", "application/x-java-applet");
            mimeMap.Add(".clp", "application/x-msclip");
            mimeMap.Add(".cmx", "image/x-cmx");
            mimeMap.Add(".cnf", "text/plain");
            mimeMap.Add(".cod", "image/cis-cod");
            mimeMap.Add(".cpio", "application/x-cpio");
            mimeMap.Add(".cpp", "text/plain");
            mimeMap.Add(".crd", "application/x-mscardfile");
            mimeMap.Add(".crl", "application/pkix-crl");
            mimeMap.Add(".crt", "application/x-x509-ca-cert");
            mimeMap.Add(".csh", "application/x-csh");
            mimeMap.Add(".css", "text/css");
            mimeMap.Add(".csv", "application/octet-stream");
            mimeMap.Add(".cur", "application/octet-stream");
            mimeMap.Add(".dcr", "application/x-director");
            mimeMap.Add(".deploy", "application/octet-stream");
            mimeMap.Add(".der", "application/x-x509-ca-cert");
            mimeMap.Add(".dib", "image/bmp");
            mimeMap.Add(".dir", "application/x-director");
            mimeMap.Add(".disco", "text/xml");
            mimeMap.Add(".dll", "application/x-msdownload");
            mimeMap.Add(".dll.config", "text/xml");
            mimeMap.Add(".dlm", "text/dlm");
            mimeMap.Add(".doc", "application/msword");
            mimeMap.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
            mimeMap.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            mimeMap.Add(".dot", "application/msword");
            mimeMap.Add(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
            mimeMap.Add(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            mimeMap.Add(".dsp", "application/octet-stream");
            mimeMap.Add(".dtd", "text/xml");
            mimeMap.Add(".dvi", "application/x-dvi");
            mimeMap.Add(".dwf", "drawing/x-dwf");
            mimeMap.Add(".dwp", "application/octet-stream");
            mimeMap.Add(".dxr", "application/x-director");
            mimeMap.Add(".eml", "message/rfc822");
            mimeMap.Add(".emz", "application/octet-stream");
            mimeMap.Add(".eot", "application/octet-stream");
            mimeMap.Add(".eps", "application/postscript");
            mimeMap.Add(".etx", "text/x-setext");
            mimeMap.Add(".evy", "application/envoy");
            mimeMap.Add(".exe", "application/octet-stream");
            mimeMap.Add(".exe.config", "text/xml");
            mimeMap.Add(".fdf", "application/vnd.fdf");
            mimeMap.Add(".fif", "application/fractals");
            mimeMap.Add(".fla", "application/octet-stream");
            mimeMap.Add(".flr", "x-world/x-vrml");
            mimeMap.Add(".flv", "video/x-flv");
            mimeMap.Add(".gif", "image/gif");
            mimeMap.Add(".gtar", "application/x-gtar");
            mimeMap.Add(".gz", "application/x-gzip");
            mimeMap.Add(".h", "text/plain");
            mimeMap.Add(".hdf", "application/x-hdf");
            mimeMap.Add(".hdml", "text/x-hdml");
            mimeMap.Add(".hhc", "application/x-oleobject");
            mimeMap.Add(".hhk", "application/octet-stream");
            mimeMap.Add(".hhp", "application/octet-stream");
            mimeMap.Add(".hlp", "application/winhlp");
            mimeMap.Add(".hqx", "application/mac-binhex40");
            mimeMap.Add(".hta", "application/hta");
            mimeMap.Add(".htc", "text/x-component");
            mimeMap.Add(".htm", "text/html");
            mimeMap.Add(".html", "text/html");
            mimeMap.Add(".htt", "text/webviewhtml");
            mimeMap.Add(".hxt", "text/html");
            mimeMap.Add(".ico", "image/x-icon");
            mimeMap.Add(".ics", "application/octet-stream");
            mimeMap.Add(".ief", "image/ief");
            mimeMap.Add(".iii", "application/x-iphone");
            mimeMap.Add(".inf", "application/octet-stream");
            mimeMap.Add(".ins", "application/x-internet-signup");
            mimeMap.Add(".isp", "application/x-internet-signup");
            mimeMap.Add(".IVF", "video/x-ivf");
            mimeMap.Add(".jar", "application/java-archive");
            mimeMap.Add(".java", "application/octet-stream");
            mimeMap.Add(".jck", "application/liquidmotion");
            mimeMap.Add(".jcz", "application/liquidmotion");
            mimeMap.Add(".jfif", "image/pjpeg");
            mimeMap.Add(".jpb", "application/octet-stream");
            mimeMap.Add(".jpe", "image/jpeg");
            mimeMap.Add(".jpeg", "image/jpeg");
            mimeMap.Add(".jpg", "image/jpeg");
            mimeMap.Add(".js", "application/x-javascript");
            mimeMap.Add(".jsx", "text/jscript");
            mimeMap.Add(".latex", "application/x-latex");
            mimeMap.Add(".lit", "application/x-ms-reader");
            mimeMap.Add(".lpk", "application/octet-stream");
            mimeMap.Add(".lsf", "video/x-la-asf");
            mimeMap.Add(".lsx", "video/x-la-asf");
            mimeMap.Add(".lzh", "application/octet-stream");
            mimeMap.Add(".m13", "application/x-msmediaview");
            mimeMap.Add(".m14", "application/x-msmediaview");
            mimeMap.Add(".m1v", "video/mpeg");
            mimeMap.Add(".m3u", "audio/x-mpegurl");
            mimeMap.Add(".man", "application/x-troff-man");
            mimeMap.Add(".manifest", "application/x-ms-manifest");
            mimeMap.Add(".map", "text/plain");
            mimeMap.Add(".mdb", "application/x-msaccess");
            mimeMap.Add(".mdp", "application/octet-stream");
            mimeMap.Add(".me", "application/x-troff-me");
            mimeMap.Add(".mht", "message/rfc822");
            mimeMap.Add(".mhtml", "message/rfc822");
            mimeMap.Add(".mid", "audio/mid");
            mimeMap.Add(".midi", "audio/mid");
            mimeMap.Add(".mix", "application/octet-stream");
            mimeMap.Add(".mmf", "application/x-smaf");
            mimeMap.Add(".mno", "text/xml");
            mimeMap.Add(".mny", "application/x-msmoney");
            mimeMap.Add(".mov", "video/quicktime");
            mimeMap.Add(".movie", "video/x-sgi-movie");
            mimeMap.Add(".mp2", "video/mpeg");
            mimeMap.Add(".mp3", "audio/mpeg");
            mimeMap.Add(".mpa", "video/mpeg");
            mimeMap.Add(".mpe", "video/mpeg");
            mimeMap.Add(".mpeg", "video/mpeg");
            mimeMap.Add(".mpg", "video/mpeg");
            mimeMap.Add(".mpp", "application/vnd.ms-project");
            mimeMap.Add(".mpv2", "video/mpeg");
            mimeMap.Add(".ms", "application/x-troff-ms");
            mimeMap.Add(".msi", "application/octet-stream");
            mimeMap.Add(".mso", "application/octet-stream");
            mimeMap.Add(".mvb", "application/x-msmediaview");
            mimeMap.Add(".mvc", "application/x-miva-compiled");
            mimeMap.Add(".nc", "application/x-netcdf");
            mimeMap.Add(".nsc", "video/x-ms-asf");
            mimeMap.Add(".nws", "message/rfc822");
            mimeMap.Add(".ocx", "application/octet-stream");
            mimeMap.Add(".oda", "application/oda");
            mimeMap.Add(".odc", "text/x-ms-odc");
            mimeMap.Add(".ods", "application/oleobject");
            mimeMap.Add(".one", "application/onenote");
            mimeMap.Add(".onea", "application/onenote");
            mimeMap.Add(".onetoc", "application/onenote");
            mimeMap.Add(".onetoc2", "application/onenote");
            mimeMap.Add(".onetmp", "application/onenote");
            mimeMap.Add(".onepkg", "application/onenote");
            mimeMap.Add(".osdx", "application/opensearchdescription+xml");
            mimeMap.Add(".p10", "application/pkcs10");
            mimeMap.Add(".p12", "application/x-pkcs12");
            mimeMap.Add(".p7b", "application/x-pkcs7-certificates");
            mimeMap.Add(".p7c", "application/pkcs7-mime");
            mimeMap.Add(".p7m", "application/pkcs7-mime");
            mimeMap.Add(".p7r", "application/x-pkcs7-certreqresp");
            mimeMap.Add(".p7s", "application/pkcs7-signature");
            mimeMap.Add(".pbm", "image/x-portable-bitmap");
            mimeMap.Add(".pcx", "application/octet-stream");
            mimeMap.Add(".pcz", "application/octet-stream");
            mimeMap.Add(".pdf", "application/pdf");
            mimeMap.Add(".pfb", "application/octet-stream");
            mimeMap.Add(".pfm", "application/octet-stream");
            mimeMap.Add(".pfx", "application/x-pkcs12");
            mimeMap.Add(".pgm", "image/x-portable-graymap");
            mimeMap.Add(".pko", "application/vnd.ms-pki.pko");
            mimeMap.Add(".pma", "application/x-perfmon");
            mimeMap.Add(".pmc", "application/x-perfmon");
            mimeMap.Add(".pml", "application/x-perfmon");
            mimeMap.Add(".pmr", "application/x-perfmon");
            mimeMap.Add(".pmw", "application/x-perfmon");
            mimeMap.Add(".png", "image/png");
            mimeMap.Add(".pnm", "image/x-portable-anymap");
            mimeMap.Add(".pnz", "image/png");
            mimeMap.Add(".pot", "application/vnd.ms-powerpoint");
            mimeMap.Add(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
            mimeMap.Add(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
            mimeMap.Add(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
            mimeMap.Add(".ppm", "image/x-portable-pixmap");
            mimeMap.Add(".pps", "application/vnd.ms-powerpoint");
            mimeMap.Add(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            mimeMap.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            mimeMap.Add(".ppt", "application/vnd.ms-powerpoint");
            mimeMap.Add(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            mimeMap.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            mimeMap.Add(".prf", "application/pics-rules");
            mimeMap.Add(".prm", "application/octet-stream");
            mimeMap.Add(".prx", "application/octet-stream");
            mimeMap.Add(".ps", "application/postscript");
            mimeMap.Add(".psd", "application/octet-stream");
            mimeMap.Add(".psm", "application/octet-stream");
            mimeMap.Add(".psp", "application/octet-stream");
            mimeMap.Add(".pub", "application/x-mspublisher");
            mimeMap.Add(".qt", "video/quicktime");
            mimeMap.Add(".qtl", "application/x-quicktimeplayer");
            mimeMap.Add(".qxd", "application/octet-stream");
            mimeMap.Add(".ra", "audio/x-pn-realaudio");
            mimeMap.Add(".ram", "audio/x-pn-realaudio");
            mimeMap.Add(".rar", "application/octet-stream");
            mimeMap.Add(".ras", "image/x-cmu-raster");
            mimeMap.Add(".rf", "image/vnd.rn-realflash");
            mimeMap.Add(".rgb", "image/x-rgb");
            mimeMap.Add(".rm", "application/vnd.rn-realmedia");
            mimeMap.Add(".rmi", "audio/mid");
            mimeMap.Add(".roff", "application/x-troff");
            mimeMap.Add(".rpm", "audio/x-pn-realaudio-plugin");
            mimeMap.Add(".rtf", "application/rtf");
            mimeMap.Add(".rtx", "text/richtext");
            mimeMap.Add(".scd", "application/x-msschedule");
            mimeMap.Add(".sct", "text/scriptlet");
            mimeMap.Add(".sea", "application/octet-stream");
            mimeMap.Add(".setpay", "application/set-payment-initiation");
            mimeMap.Add(".setreg", "application/set-registration-initiation");
            mimeMap.Add(".sgml", "text/sgml");
            mimeMap.Add(".sh", "application/x-sh");
            mimeMap.Add(".shar", "application/x-shar");
            mimeMap.Add(".sit", "application/x-stuffit");
            mimeMap.Add(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12");
            mimeMap.Add(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
            mimeMap.Add(".smd", "audio/x-smd");
            mimeMap.Add(".smi", "application/octet-stream");
            mimeMap.Add(".smx", "audio/x-smd");
            mimeMap.Add(".smz", "audio/x-smd");
            mimeMap.Add(".snd", "audio/basic");
            mimeMap.Add(".snp", "application/octet-stream");
            mimeMap.Add(".spc", "application/x-pkcs7-certificates");
            mimeMap.Add(".spl", "application/futuresplash");
            mimeMap.Add(".src", "application/x-wais-source");
            mimeMap.Add(".ssm", "application/streamingmedia");
            mimeMap.Add(".sst", "application/vnd.ms-pki.certstore");
            mimeMap.Add(".stl", "application/vnd.ms-pki.stl");
            mimeMap.Add(".sv4cpio", "application/x-sv4cpio");
            mimeMap.Add(".sv4crc", "application/x-sv4crc");
            mimeMap.Add(".swf", "application/x-shockwave-flash");
            mimeMap.Add(".t", "application/x-troff");
            mimeMap.Add(".tar", "application/x-tar");
            mimeMap.Add(".tcl", "application/x-tcl");
            mimeMap.Add(".tex", "application/x-tex");
            mimeMap.Add(".texi", "application/x-texinfo");
            mimeMap.Add(".texinfo", "application/x-texinfo");
            mimeMap.Add(".tgz", "application/x-compressed");
            mimeMap.Add(".thmx", "application/vnd.ms-officetheme");
            mimeMap.Add(".thn", "application/octet-stream");
            mimeMap.Add(".tif", "image/tiff");
            mimeMap.Add(".tiff", "image/tiff");
            mimeMap.Add(".toc", "application/octet-stream");
            mimeMap.Add(".tr", "application/x-troff");
            mimeMap.Add(".trm", "application/x-msterminal");
            mimeMap.Add(".tsv", "text/tab-separated-values");
            mimeMap.Add(".ttf", "application/octet-stream");
            mimeMap.Add(".txt", "text/plain");
            mimeMap.Add(".u32", "application/octet-stream");
            mimeMap.Add(".uls", "text/iuls");
            mimeMap.Add(".ustar", "application/x-ustar");
            mimeMap.Add(".vbs", "text/vbscript");
            mimeMap.Add(".vcf", "text/x-vcard");
            mimeMap.Add(".vcs", "text/plain");
            mimeMap.Add(".vdx", "application/vnd.ms-visio.viewer");
            mimeMap.Add(".vml", "text/xml");
            mimeMap.Add(".vsd", "application/vnd.visio");
            mimeMap.Add(".vss", "application/vnd.visio");
            mimeMap.Add(".vst", "application/vnd.visio");
            mimeMap.Add(".vsto", "application/x-ms-vsto");
            mimeMap.Add(".vsw", "application/vnd.visio");
            mimeMap.Add(".vsx", "application/vnd.visio");
            mimeMap.Add(".vtx", "application/vnd.visio");
            mimeMap.Add(".wav", "audio/wav");
            mimeMap.Add(".wax", "audio/x-ms-wax");
            mimeMap.Add(".wbmp", "image/vnd.wap.wbmp");
            mimeMap.Add(".wcm", "application/vnd.ms-works");
            mimeMap.Add(".wdb", "application/vnd.ms-works");
            mimeMap.Add(".wks", "application/vnd.ms-works");
            mimeMap.Add(".wm", "video/x-ms-wm");
            mimeMap.Add(".wma", "audio/x-ms-wma");
            mimeMap.Add(".wmd", "application/x-ms-wmd");
            mimeMap.Add(".wmf", "application/x-msmetafile");
            mimeMap.Add(".wml", "text/vnd.wap.wml");
            mimeMap.Add(".wmlc", "application/vnd.wap.wmlc");
            mimeMap.Add(".wmls", "text/vnd.wap.wmlscript");
            mimeMap.Add(".wmlsc", "application/vnd.wap.wmlscriptc");
            mimeMap.Add(".wmp", "video/x-ms-wmp");
            mimeMap.Add(".wmv", "video/x-ms-wmv");
            mimeMap.Add(".wmx", "video/x-ms-wmx");
            mimeMap.Add(".wmz", "application/x-ms-wmz");
            mimeMap.Add(".wps", "application/vnd.ms-works");
            mimeMap.Add(".wri", "application/x-mswrite");
            mimeMap.Add(".wrl", "x-world/x-vrml");
            mimeMap.Add(".wrz", "x-world/x-vrml");
            mimeMap.Add(".wsdl", "text/xml");
            mimeMap.Add(".wvx", "video/x-ms-wvx");
            mimeMap.Add(".x", "application/directx");
            mimeMap.Add(".xaf", "x-world/x-vrml");
            mimeMap.Add(".xaml", "application/xaml+xml");
            mimeMap.Add(".xap", "application/x-silverlight-app");
            mimeMap.Add(".xbap", "application/x-ms-xbap");
            mimeMap.Add(".xbm", "image/x-xbitmap");
            mimeMap.Add(".xdr", "text/plain");
            mimeMap.Add(".xla", "application/vnd.ms-excel");
            mimeMap.Add(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
            mimeMap.Add(".xlc", "application/vnd.ms-excel");
            mimeMap.Add(".xlm", "application/vnd.ms-excel");
            mimeMap.Add(".xls", "application/vnd.ms-excel");
            mimeMap.Add(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            mimeMap.Add(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
            mimeMap.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            mimeMap.Add(".xlt", "application/vnd.ms-excel");
            mimeMap.Add(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
            mimeMap.Add(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            mimeMap.Add(".xlw", "application/vnd.ms-excel");
            mimeMap.Add(".xml", "text/xml");
            mimeMap.Add(".xof", "x-world/x-vrml");
            mimeMap.Add(".xpm", "image/x-xpixmap");
            mimeMap.Add(".xps", "application/vnd.ms-xpsdocument");
            mimeMap.Add(".xsd", "text/xml");
            mimeMap.Add(".xsf", "text/xml");
            mimeMap.Add(".xsl", "text/xml");
            mimeMap.Add(".xslt", "text/xml");
            mimeMap.Add(".xsn", "application/octet-stream");
            mimeMap.Add(".xtp", "application/octet-stream");
            mimeMap.Add(".xwd", "image/x-xwindowdump");
            mimeMap.Add(".z", "application/x-compress");
            mimeMap.Add(".zip", "application/x-zip-compressed");
        }
        public static string GetMimeMapping(string fileName)
        {
            try
            {
                return mimeMap[Path.GetExtension(fileName)];
            }
            catch (KeyNotFoundException)
            {
                return "application/octet-stream";
            }
        }
    }
}