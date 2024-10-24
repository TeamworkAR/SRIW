﻿using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

namespace AeLa.EasyFeedback.Web
{
	/// <summary>
	/// Handles constructing and sending all EF web requests.
	/// </summary>
	internal static class WebInterface
	{
		/// <summary>
		/// Sends a GET request, and blocks the main thread until a response is returned
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="onStatusUpdate">Called while blocking the main thread to allow for updating</param>
		/// <returns>The response returned from the request</returns>
		public static WebResponse Get(
			string uri,
			Action<AsyncWebRequestData> onStatusUpdate = null
		)
		{
			return WaitForResponse(MakeGet(uri), onStatusUpdate);
		}

		/// <summary>
		/// Sends a GET request, waits for a response, and calls <paramref name="onResponseReturned"/> with the returned response
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="onResponseReturned">Called when the request has finished and a response has been returned</param>
		public static IEnumerator GetCoroutine(
			string uri,
			Action<WebResponse> onResponseReturned
		)
		{
			return WaitForResponseCoroutine(MakeGet(uri), onResponseReturned);
		}

		/// <summary>
		/// Sends a POST request, and blocks the main thread until a response is returned
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="data">The request payload</param>
		/// <param name="onStatusUpdate">Called while blocking the main thread to allow for updating</param>
		/// <returns>The response returned from the request</returns>
		public static WebResponse Post(
			string uri,
			WWWForm data,
			Action<AsyncWebRequestData> onStatusUpdate = null
		)
		{
			return WaitForResponse(MakePost(uri, data), onStatusUpdate);
		}

		/// <summary>
		/// Sends a POST request, and blocks the main thread until a response is returned
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <param name="onStatusUpdate">Called while blocking the main thread to allow for updating</param>
		/// <returns>The response returned from the request</returns>
		public static WebResponse Post(
			string uri,
			string contentType,
			byte[] data,
			Action<AsyncWebRequestData> onStatusUpdate = null
		)
		{
			return WaitForResponse(
				MakePost(uri, contentType, data), onStatusUpdate
			);
		}

		/// <summary>
		/// Sends a POST request, waits for a response, and calls <paramref name="onResponseReturned"/> with the returned response
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="data">The request payload</param>
		/// <param name="onResponseReturned">Called when the request has finished and a response has been returned</param>
		public static IEnumerator PostCoroutine(
			string uri,
			WWWForm data,
			Action<WebResponse> onResponseReturned
		)
		{
			return WaitForResponseCoroutine(
				MakePost(uri, data), onResponseReturned
			);
		}

		/// <summary>
		/// Sends a POST request, waits for a response, and calls <paramref name="onResponseReturned"/> with the returned response
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <param name="onResponseReturned">Called when the request has finished and a response has been returned</param>
		public static IEnumerator PostCoroutine(
			string uri,
			string contentType,
			byte[] data,
			Action<WebResponse> onResponseReturned
		)
		{
			return WaitForResponseCoroutine(
				MakePost(uri, contentType, data), onResponseReturned
			);
		}

		/// <summary>
		/// Sends a PUT request, and blocks the main thread until a response is returned
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <param name="onStatusUpdate">Called while blocking the main thread to allow for updating</param>
		/// <returns>The response returned from the request</returns>
		public static WebResponse Put(
			string uri,
			string contentType = null,
			byte[] data = null,
			Action<AsyncWebRequestData> onStatusUpdate = null
		)
		{
			return WaitForResponse(
				MakePut(uri, contentType, data), onStatusUpdate
			);
		}

		/// <summary>
		/// Sends a PUT request, waits for a response, and calls <paramref name="onResponseReturned"/> with the returned response
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <param name="onResponseReturned">Called when the request has finished and a response has been returned</param>
		public static IEnumerator PutCoroutine(
			string uri,
			string contentType,
			byte[] data,
			Action<WebResponse> onResponseReturned
		)
		{
			return WaitForResponseCoroutine(
				MakePut(uri, contentType, data), onResponseReturned
			);
		}

		/// <summary>
		/// Sends a GET request to the provided <paramref name="uri"/>
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <returns>The web request and async operation</returns>
		private static AsyncWebRequestData MakeGet(string uri)
		{
			return MakeRequest(uri, WebRequestMethod.GET);
		}

		/// <summary>
		/// Sends a POST request to the provided <paramref name="uri"/>
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="data">The request payload</param>
		/// <returns>The web request and async operation</returns>
		private static AsyncWebRequestData MakePost(string uri, WWWForm data)
		{
			return MakeRequest(uri, data);
		}

		/// <summary>
		/// Sends a POST request to the provided <paramref name="uri"/>
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <returns>The web request and async operation</returns>
		private static AsyncWebRequestData MakePost(
			string uri, string contentType, byte[] data
		)
		{
			return MakeRequest(
				uri,
				WebRequestMethod.POST,
				contentType,
				data
			);
		}

		/// <summary>
		/// Sends a PUT request to the provided <paramref name="uri"/>
		/// </summary>
		/// <param name="uri">The requested resource</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <returns>The web request and async operation</returns>
		private static AsyncWebRequestData MakePut(
			string uri, string contentType = null, byte[] data = null
		)
		{
			return MakeRequest(
				uri,
				WebRequestMethod.PUT,
				contentType,
				data
			);
		}

		/// <summary>
		/// Constructs and sends a web request with the provided <paramref name="method"/>.
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="method">The request method</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload</param>
		/// <returns>The web request and async operation</returns>
		private static AsyncWebRequestData MakeRequest(
			string uri,
			WebRequestMethod method,
			string contentType = null,
			byte[] data = null
		)
		{
			CheckCertificateValidationCallback();

			// construct UnityWebRequest
			var request = ConstructWebRequest(
				uri, method, contentType, data
			);

			// send request and return data
			return new AsyncWebRequestData(request, SendWebRequest(request));
		}

		/// <summary>
		/// Constructs and POSTs a web request with the provided <paramref name="data"/>.
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="data">The request payload</param>
		private static AsyncWebRequestData MakeRequest(
			string uri,
			WWWForm data
		)
		{
			CheckCertificateValidationCallback();

			var request = ConstructWebRequest(uri, data);


#pragma warning disable 0618 // deprecation warning for chunkedTransfer
			request.chunkedTransfer = false;
#pragma warning restore 0618

			return new AsyncWebRequestData(request, SendWebRequest(request));
		}

		/// <summary>
		/// Blocks the main thread until the web request is finished
		/// </summary>
		/// <param name="requestData">The web request</param>
		/// <param name="onStatusUpdate">Called while blocking the main thread to allow for updating</param>
		/// <returns>The response from the web request</returns>
		private static WebResponse WaitForResponse(
			AsyncWebRequestData requestData,
			Action<AsyncWebRequestData> onStatusUpdate = null
		)
		{
			while (!requestData.OperationIsDone)
				if (onStatusUpdate != null)
				{
					onStatusUpdate(requestData);
				}

			return WebResponse.GetResponse(requestData);
		}

		/// <summary>
		/// Waits until the request has finished, and calls <paramref name="onResponseReturned"/> with the returned response
		/// </summary>
		/// <param name="requestData">The web request</param>
		/// <param name="onResponseReturned">Called when the request has finished and a response has been returned</param>
		private static IEnumerator WaitForResponseCoroutine(
			AsyncWebRequestData requestData,
			Action<WebResponse> onResponseReturned = null
		)
		{
			while (!requestData.OperationIsDone) yield return null;

			if (onResponseReturned != null)
			{
				onResponseReturned(WebResponse.GetResponse(requestData));
			}

			requestData.Request.Dispose();
		}

		/// <summary>
		/// Constructs a web request
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="method">The request method</param>
		/// <param name="contentType">The content type of the request payload</param>
		/// <param name="data">The request payload. Will only be sent when <paramref name="method"/> is <see cref="WebRequestMethod.POST"/> or <see cref="WebRequestMethod.PUT"/></param>
		/// <returns>The web request</returns>
		private static UnityWebRequest ConstructWebRequest(
			string uri,
			WebRequestMethod method,
			string contentType = null,
			byte[] data = null
		)
		{
			// create the request
			var request = new UnityWebRequest(
				uri, GetRequestMethodString(method)
			) { downloadHandler = new DownloadHandlerBuffer() };

			// add a download handler

			// write data to stream and set content type
			if (data != null)
			{
				UploadHandler handler = new UploadHandlerRaw(data);
				handler.contentType = contentType;
				request.uploadHandler = handler;
			}
			// set content type only if no data
			else if (contentType != null)
			{
				request.SetRequestHeader(
					Constants.Web.Header.ContentType, contentType
				);
			}

			return request;
		}

		/// <summary>
		/// Constructs a POST web request with the provided <paramref name="data"/>.
		/// </summary>
		/// <param name="uri">The URI of the requested resource</param>
		/// <param name="data">The request payload</param>
		private static UnityWebRequest ConstructWebRequest(
			string uri,
			WWWForm data
		)
		{
			return UnityWebRequest.Post(uri, data);
		}

		/// <summary>
		/// Sends the web request and returns the appropriate async operation type for the current API
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private static AsyncOperation SendWebRequest(UnityWebRequest request)
		{
			// we're keeping a reference to the underlying web request with AsyncWebRequestData
			// so, we can cast UnityWebRequestAsyncOperation down to just AsyncOperation for simplicity
			return request.SendWebRequest();
		}

		/// <summary>
		/// Returns the string equivalent of a WebRequestMethod value
		/// </summary>
		private static string GetRequestMethodString(WebRequestMethod method)
		{
			switch (method)
			{
				case WebRequestMethod.GET:
					return Constants.Web.RequestMethod.GET;
				case WebRequestMethod.POST:
					return Constants.Web.RequestMethod.POST;
				case WebRequestMethod.PUT:
					return Constants.Web.RequestMethod.PUT;
				default:
					throw new NotImplementedException(
						"No string conversion for provided WebRequestMethod value."
					);
			}
		}

		/// <summary>
		/// Checks if the ServerCertificateValidationCallback is set yet, and sets it if not.
		/// </summary>
		private static void CheckCertificateValidationCallback()
		{
			// set remote certificate validation callback if not set already
			if (ServicePointManager.ServerCertificateValidationCallback !=
			    RemoteCertificateValidationCallback)
			{
				ServicePointManager.ServerCertificateValidationCallback =
					RemoteCertificateValidationCallback;
			}
		}

		/// <remarks>
		/// see: https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="sslPolicyErrors"></param>
		/// <returns></returns>
		private static bool RemoteCertificateValidationCallback(
			object sender, X509Certificate certificate, X509Chain chain,
			SslPolicyErrors sslPolicyErrors
		)
		{
			var isOk = true;
			// If there are errors in the certificate chain, look at each error to determine the cause.
			if (sslPolicyErrors != SslPolicyErrors.None)
			{
				for (var i = 0; i < chain.ChainStatus.Length; i++)
					if (chain.ChainStatus[i].Status !=
					    X509ChainStatusFlags.RevocationStatusUnknown)
					{
						chain.ChainPolicy.RevocationFlag =
							X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode =
							X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout =
							new TimeSpan(0, 1, 0);
						chain.ChainPolicy.VerificationFlags =
							X509VerificationFlags.AllFlags;
						var chainIsValid =
							chain.Build((X509Certificate2)certificate);
						if (!chainIsValid)
						{
							isOk = false;
						}
					}
			}

			return isOk;
		}
	}
}