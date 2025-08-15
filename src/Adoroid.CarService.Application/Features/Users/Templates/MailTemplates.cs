namespace Adoroid.CarService.Application.Features.Users.Templates;

public static class MailTemplates
{
    public const string GetVerificationEmailTemplate = $@"<!doctype html>
<html lang=""tr"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <meta charset=""utf-8"">
  <meta name=""x-apple-disable-message-reformatting"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <title>FixyBear Doğrulama</title>
  <style>
    .preheader {{ display:none !important; visibility:hidden; opacity:0; color:transparent; height:0; width:0; overflow:hidden; mso-hide:all; }}
    @media (prefers-color-scheme: dark) {{
      body, .email-bg {{ background:#0b1220 !important; }}
      .card {{ background:#0f172a !important; border-color:#1f2a44 !important; }}
      .text {{ color:#e5e7eb !important; }}
      .muted {{ color:#9aa4b2 !important; }}
      .btn {{ background:#6366f1 !important; color:#ffffff !important; }}
      .code {{ background:#111827 !important; color:#e5e7eb !important; border-color:#374151 !important; }}
    }}
  </style>
</head>
<body style=""margin:0; padding:0; background:#f3f4f6;"">
  <div class=""preheader"">
    Doğrulama kodunuz: {{{{OTP_CODE}}}}
  </div>

  <!-- Outer wrapper -->
  <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""email-bg"" style=""background:#f3f4f6; padding:24px 0;"">
    <tr>
      <td align=""center"">
        <!-- Container -->
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""width:600px; max-width:100%; font-family: ui-sans-serif, -apple-system, Segoe UI, Tahoma, Arial, sans-serif;"">
          <!-- Header -->
          <tr>
            <td align=""left"" style=""padding:24px 24px 8px 24px;"">
              <div style=""font-size:24px; line-height:1.2; font-weight:700; color:#111827;"">
                FixyBear
              </div>
              <div class=""muted"" style=""font-size:12px; color:#6b7280; margin-top:4px;"">
                Hesap doğrulama kodunuz aşağıdadır.
              </div>
            </td>
          </tr>

          <!-- Card -->
          <tr>
            <td>
              <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""card"" style=""background:#ffffff; border:1px solid #e5e7eb; border-radius:12px; overflow:hidden;"">
                <tr>
                  <td style=""padding:24px;"">
                    <h1 class=""text"" style=""margin:0 0 8px 0; font-size:20px; line-height:1.4; color:#111827;"">
                      Hesabınızı aktifleştirin
                    </h1>
                    <p class=""text"" style=""margin:0 0 16px 0; font-size:14px; color:#374151;"">
                      FixyBear’a kaydolduğunuz için teşekkür ederiz. Lütfen aşağıdaki tek kullanımlık doğrulama kodunu uygulamaya girerek hesabınızı aktifleştirin.
                    </p>

                    <!-- OTP Code -->
                    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""margin:12px 0 20px 0;"">
                      <tr>
                        <td align=""center"">
                          <div class=""code"" style=""display:inline-block; font-family:ui-monospace, SFMono-Regular, Menlo, Consolas, monospace; letter-spacing:6px; font-weight:700; font-size:28px; padding:14px 18px; border:1px solid #e5e7eb; border-radius:10px; background:#f9fafb; color:#111827;"">
                            {{{{OTP_CODE}}}}
                          </div>
                        </td>
                      </tr>
                    </table>

                    <!-- Button -->
                    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" align=""center"" style=""margin:0 auto 12px auto;"">
                      <tr>
                        <td align=""center"" bgcolor=""#4f46e5"" class=""btn"" style=""border-radius:10px;"">
                          <a href=""{{{{ACTION_URL}}}}"" target=""_blank"" style=""display:inline-block; padding:12px 18px; font-size:14px; font-weight:600; text-decoration:none; color:#ffffff;"">
                            Hesabı Doğrula
                          </a>
                        </td>
                      </tr>
                    </table>

                    <p class=""muted"" style=""margin:8px 0 0 0; font-size:12px; color:#6b7280; text-align:center;"">
                      Buton çalışmıyorsa bu bağlantıyı tarayıcınıza yapıştırın: <br>
                      <a href=""{{{{ACTION_URL}}}}"" style=""color:#4f46e5; text-decoration:underline; word-break:break-all;"">{{{{ACTION_URL}}}}</a>
                    </p>

                    <!-- Meta -->
                    <hr style=""border:none; border-top:1px solid #e5e7eb; margin:20px 0;"">
                    <p class=""text"" style=""margin:0 0 6px 0; font-size:13px; color:#374151;"">
                      Güvenliğiniz için bu kodu <strong>kimseyle paylaşmayın</strong>.
                    </p>
                    <p class=""text"" style=""margin:0; font-size:13px; color:#374151;"">
                      Bu isteği siz başlatmadıysanız, bu e-postayı yok sayabilirsiniz.
                    </p>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Footer -->
          <tr>
            <td align=""center"" style=""padding:16px 24px 0 24px;"">
              <p class=""muted"" style=""margin:0; font-size:12px; color:#6b7280;"">
                Sorularınız için <a href=""mailto:{{{{SUPPORT_EMAIL}}}}"" style=""color:#4f46e5; text-decoration:underline;"">{{{{SUPPORT_EMAIL}}}}</a>
              </p>
              <p class=""muted"" style=""margin:6px 0 0 0; font-size:12px; color:#6b7280;"">
                © {{{{CURRENT_YEAR}}}} FixyBear. Tüm hakları saklıdır.
              </p>
              <p class=""muted"" style=""margin:6px 0 0 0; font-size:12px; color:#6b7280;"">
                <a href=""https://fixybear.com"" style=""color:#6b7280; text-decoration:none;"">fixybear.com</a> · <a href=""{{{{PRIVACY_URL}}}}"" style=""color:#6b7280; text-decoration:underline;"">Gizlilik</a>
              </p>
            </td>
          </tr>
          <tr>
            <td style=""height:24px;""></td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
}
