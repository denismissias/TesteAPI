using API.Data;
using API.Model;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using System;

namespace API
{
    public class APIModule : NancyModule
    {
        public APIModule()
        {
            Get["/{id:guid}"] = parameters =>
            {
                APIData data = new APIData();

                APIModel model = data.GetById(parameters.id);

                if (model != null)
                {
                    return Response.AsJson(model);
                }

                return HttpStatusCode.NotFound;
            };

            Post["/"] = _ =>
            {
                APIModelBinding modelBinding = this.Bind();

                Guid testeGuid;
                Boolean isGuid = Guid.TryParse(modelBinding.Id, out testeGuid);

                if (!isGuid)
                {
                    JsonResponse response = new JsonResponse("InvalidGuid", new DefaultJsonSerializer());
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ReasonPhrase = "Invalid Guid";
                    return response;
                }

                Uri testeURI;
                Boolean isURI = Uri.TryCreate(modelBinding.Url, UriKind.Absolute, out testeURI)
                    && (testeURI.Scheme == Uri.UriSchemeHttp || testeURI.Scheme == Uri.UriSchemeHttps);

                if (!isURI)
                {
                    JsonResponse response = new JsonResponse("InvalidURL", new DefaultJsonSerializer());
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ReasonPhrase = "Invalid URL";
                    return response;
                }

                DateTime testeDateTime;
                Boolean isDateTime = DateTime.TryParse(modelBinding.CreateDate, out testeDateTime);

                if (!isDateTime)
                {
                    JsonResponse response = new JsonResponse("InvalidDateTime", new DefaultJsonSerializer());
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ReasonPhrase = "Invalid DateTime";
                    return response;
                }

                APIModel model = new APIModel
                {
                    Id = modelBinding.Id,
                    Url = modelBinding.Url,
                    CreateDate = testeDateTime
                };

                APIData data = new APIData();

                String resultado = data.SaveAPI(model);

                if (resultado.ToUpper().Equals("OK"))
                {
                    return HttpStatusCode.Created;
                }
                else
                {
                    return HttpStatusCode.BadRequest;
                }
            };
        }
    }
}