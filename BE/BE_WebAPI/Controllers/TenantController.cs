﻿using BE.Data;
using BE.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace BE_WebAPI.Controllers
{
    public class TenantController : ApiController
    {
        [ActionName("GetTenant")]
        [HttpGet]
        public IHttpActionResult GetTenant(string id)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                Tenant _tenant = _userOperation.GetTenantDetails(Convert.ToInt32(id));
                if (_tenant == null)
                    return NotFound();
                else
                    return Ok(_tenant);
            }
        }

        [ActionName("GetEmployee")]
        [HttpGet]
        public IHttpActionResult GetEmployee(string id)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                User _user = _userOperation.GetEmployeeDetails(Convert.ToInt32(id));
                if (_user == null)
                    return NotFound();
                else
                    return Ok(_user);
            }
        }

        [ActionName("GetReceiptDetails")]
        [HttpGet]
        public IHttpActionResult GetReceiptDetails(string id)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                IEnumerable<Receipt> _receiptDetails = _userOperation.GetReceiptDetails(Convert.ToInt32(id));
                if (_receiptDetails == null || _receiptDetails.Count() == 0)
                    return NotFound();
                else
                    return Ok(_receiptDetails);
            }
        }

        [HttpGet]
        [ActionName("DownloadFile")]
        public HttpResponseMessage Download(string name)
        {
            try
            {
                string fileName = string.Empty;
                if (name.Equals("pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileName = "SamplePdf.pdf";
                }
                else if (name.Equals("zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileName = "SampleZip.zip";
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    string filePath = HttpContext.Current.Server.MapPath("~/App_Data/") + fileName;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[file.Length];
                            file.Read(bytes, 0, (int)file.Length);
                            ms.Write(bytes, 0, (int)file.Length);

                            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                            httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                            httpResponseMessage.Content.Headers.Add("x-filename", fileName);
                            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                            httpResponseMessage.StatusCode = HttpStatusCode.OK;
                            return httpResponseMessage;
                        }
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ActionName("GetComplaints")]
        public IHttpActionResult GetComplaints(string id)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                IEnumerable<Complaint> _complaintDetails = _userOperation.GetComplaintDetails(Convert.ToInt32(id));
                if (_complaintDetails == null || _complaintDetails.Count() == 0)
                    return NotFound();
                else
                    return Ok(_complaintDetails);
            }
        }

        [HttpPost]
        [ActionName("PostComplaint")]
        public HttpResponseMessage SaveComplaint([FromBody]Complaint _complaint)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                if (_userOperation.SaveComplaint(_complaint))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                }
            }
        }

        [HttpPut]        
        [ActionName("UpdateTenant")]
        public HttpResponseMessage UpdateTenant([FromBody]Tenant _tenant)
        {
            using (UserOperations _userOperation = new UserOperations())
            {
                _userOperation.UpdateTenantDetails(_tenant);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [HttpOptions]
        public void Options()
        {

        }
    }
}