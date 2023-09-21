using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        int saveUser(Usuario miobj);
        IEnumerable<Usuario> getAllUsers();
        IEnumerable<Usuario> getUserByNom(string nom);
        bool deleteUser(Usuario miobj);
        Usuario getById(int id);
        bool cantMaxUsers();
        int updateAlerta(Usuario miobj);
        IEnumerable<Usuario> getUsersTrafico();
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        { 
            new User { Id = 1, FirstName = "Test55", LastName = "User55", Username = "test", Password = "test" } 
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        private const string initVector = "mnopqrstuggabcde";
        private const int keysize = 256;

        public static string Encriptar(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            //RijndaelManaged symmetricKey = new RijndaelManaged();
            using var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Desencriptar(string cipherText, string passPhrase)
        {
            string initVector = "mnopqrstuggabcde";
            const int keysize = 256;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            //RijndaelManaged symmetricKey = new RijndaelManaged();
            using var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public User Authenticate(string username, string password)
        {
            // var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            //Solo testing
            //username = "a";
            //password = "lmn2011$";


            //Usuario uMMASS = Usuario.getByNombre(username);
            User user= null;


            Usuario uMMASS = Usuario.getByNombre(username);
            if (uMMASS != null)
            {
                if (Desencriptar(uMMASS.Clave_web, "silverblue") == password) //Heredado MMASS GRAPH
                {
                    user = new User();
                    user.Id = uMMASS.Id_usuario;
                    user.FirstName = uMMASS.Nombre;
                    user.Usrrol = uMMASS.Usrrol;
                }
            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _users.Select(x => {
                x.Password = null;
                return x;
            });
        }

        public int saveUser(Usuario miobj)
        {
            miobj.Clave_web = Encriptar(miobj.Clave_web, "silverblue");
            return miobj.save();
        }

        public IEnumerable<Usuario> getAllUsers()
        {
            return Usuario.getAllUsers();
        }

        public IEnumerable<Usuario> getUserByNom(string nom)
        {
            return Usuario.getByNombreList(nom);
        }

        public bool deleteUser(Usuario miobj)
        {
            return miobj.deleteUser();
        }

        public Usuario getById(int id)
        {
            Usuario user = Usuario.getById(id);
            user.Clave_web = Desencriptar(user.Clave_web, "silverblue");

            return user;
        }

        public bool cantMaxUsers()
        {
            return Usuario.cantMaxUsers();
        }

        public int updateAlerta(Usuario miobj)
        {
            return miobj.updateAlerta();
        }

        public IEnumerable<Usuario> getUsersTrafico()
        {
            return Usuario.getUsersTrafico();
        }

    }
}