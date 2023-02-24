using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace UnoTST.Services;

// var headers = new Dictionary<string, string>()
// {
//     { "", "" }
// };

public class HttpService
{
    private HttpClient _httpClient { get; set; }

    public HttpService()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) => { return true; };
        _httpClient = new HttpClient(handler);
    }

    /// <summary>
    /// Download file and save to path with name
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="outputPath"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task DownloadFileAsync(string uri, string outputPath)
    {
        if (uri != null && outputPath != null)
        {
            try
            {
                if (!uri.StartsWith("http"))
                    uri = $"https:{uri}";

                Uri uriResult;

                if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                    throw new InvalidOperationException("URI is invalid.");

                //if (!File.Exists(outputPath))
                //    throw new FileNotFoundException("File not found."
                //       , nameof(outputPath));

                byte[] fileBytes = await _httpClient.GetByteArrayAsync(uriResult);
                await File.WriteAllBytesAsync(outputPath, fileBytes);
            }
            catch (Exception ex)
            {
                Log.Logger(LogType.error, nameof(DownloadFileAsync), ex);
                throw;
            }
        }

    }


    /// <summary>
    /// Get Handler
    /// </summary>
    /// <param name="url"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> Get<T>(string url)
    {
        var retGet = await _httpClient.GetAsync(url);
        var content = await retGet.Content.ReadAsStringAsync();

        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Get<T>(string base_uri, string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{base_uri}{uri}");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Get<T>(Uri url, AuthenticationHeaderValue authorization)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        _httpClient.DefaultRequestHeaders.Authorization = authorization;
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Get<T>(string base_uri, string uri, AuthenticationHeaderValue authorization)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{base_uri}{uri}");
            _httpClient.DefaultRequestHeaders.Authorization = authorization;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR - " + e);
            throw new Exception(e.Message);
        }
    }

    public async Task<T> Get<T>(string base_uri, string uri, AuthenticationHeaderValue authorization,
        Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{base_uri}{uri}");
        _httpClient.DefaultRequestHeaders.Authorization = authorization;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }


    public async Task<T> Get<T>(Uri url, AuthenticationHeaderValue authorization, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        _httpClient.DefaultRequestHeaders.Authorization = authorization;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Get<T>(Uri url, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        //_httpClient.DefaultRequestHeaders.Authorization = authorization;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }


    /// <summary>
    /// Post handler
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> Post<T>(string url, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Post<T>(string base_uri, string uri, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Post<T>(string url, StringContent content, AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Post<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Post<T>(string url, StringContent content, AuthenticationHeaderValue authorization,
        Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Post<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization, Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }


    /// <summary>
    /// Path Handler
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> Patch<T>(Uri url, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url);
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Patch<T>(string base_uri, string uri, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"{base_uri}{uri}");
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Patch<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Content = content;
            request.Headers.Authorization = authorization;
            var retGet = await _httpClient.SendAsync(request);
            return await retGet.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception e)
        {
             Log.Logger(LogType.error,nameof(Patch),e, "HTTPService");
            throw;
        }

    }

    public async Task<T> Patch<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"{base_uri}{uri}");
            request.Content = content;
            request.Headers.Authorization = authorization;
            var retGet = await _httpClient.SendAsync(request);

            var sdsd = await retGet.Content.ReadAsStringAsync();
            Console.WriteLine($"################ DEBUG ########### RESP: {sdsd}");
            return await retGet.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<T> Patch<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization,
        Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url);
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Patch<T>(Uri url, StringContent content,
        Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url);
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        ///request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Patch<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization, Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, $"{base_uri}{uri}");
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }


    /// <summary>
    /// Delete Handler
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> Delete<T>(Uri url, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Delete<T>(string base_uri, string uri, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{base_uri}{uri}");
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Delete<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Delete<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{base_uri}{uri}");
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Delete<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization,
        Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Delete<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization, Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }


    /// <summary>
    /// Put Handler
    /// </summary>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> Put<T>(Uri url, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet?.Content?.ReadFromJsonAsync<T>();
    }

    public async Task<T> Put<T>(string base_uri, string uri, StringContent content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Put<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    
    public async Task<T> Put<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Put<T>(Uri url, StringContent content, AuthenticationHeaderValue authorization,
        Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> Put<T>(string base_uri, string uri, StringContent content,
        AuthenticationHeaderValue authorization, Dictionary<string, string> headers)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{base_uri}{uri}");
        request.Content = content;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        request.Headers.Authorization = authorization;
        var retGet = await _httpClient.SendAsync(request);
        return await retGet.Content.ReadFromJsonAsync<T>();
    }
}