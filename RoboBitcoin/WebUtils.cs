using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Vinit.BitCoiner.BLL;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace RoboProcessador
{
    public class WebUtils
    {
        public static Boolean EnviaEmail(String sPara, String sAssunto, String sbMensagem)
        {
            EntEmail objEmail = new EntEmail();
            objEmail.Ativo = true;
            objEmail.DataEnvio = DateTime.Now;
            objEmail.Email = sPara;
            objEmail.Mensagem = sbMensagem;
            objEmail.Titulo = sAssunto;

            Boolean res = false;
            try
            {

                String email_send = ConfigurationManager.AppSettings["SmtpRemetente"].ToString();
                String email_port = ConfigurationManager.AppSettings["SmtpPorta"].ToString();
                //  String email_smtp = "smtp.gmail.com";
                String email_smtp = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                Boolean email_autenticado = StringUtils.ToBoolean(ConfigurationManager.AppSettings["SmtpAutenticado"].ToString());
                String email_imap_usuario = ConfigurationManager.AppSettings["SmtpRemetente"].ToString();
                String email_imap_senha = ConfigurationManager.AppSettings["SmtpSenha"].ToString();

                SmtpClient client = new SmtpClient();
                MailAddress de = new MailAddress(email_send);
                MailAddress para = new MailAddress(sPara);
                MailMessage mailMessage = new MailMessage(de, para);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = sAssunto;
                mailMessage.Body = sbMensagem.ToString();

                SmtpClient smtpClient = new SmtpClient(email_smtp, Int32.Parse(email_port));

                if (email_autenticado)
                {
                    smtpClient.UseDefaultCredentials = false;
                    NetworkCredential Credentials = new NetworkCredential(email_imap_usuario, email_imap_senha);
                    smtpClient.Credentials = Credentials;
                    smtpClient.EnableSsl = true;
                }

                smtpClient.Send(mailMessage);

                objEmail.IsEnvioSucesso = true;
                res = true;
            }
            catch (Exception ex)
            {
            }

            new BllEmail().Inserir(objEmail, EntUsuario.USUARIO_PADRAO);

            return res;
        }

    }
}