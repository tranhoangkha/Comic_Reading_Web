﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigShool_1911065248.Models;

namespace BigShool_1911065248.Controllers
{
    public class FollowingsController : ApiController
    {
       
        BigShoolContext context = new BigShoolContext();  
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            //user login là người theo dõi, follow.FolloweeId là người được theo dõi
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Please login first!");
            if (userID == follow.FolloweeId)
                return BadRequest("Can not follow myself!");
            BigShoolContext context = new BigShoolContext();
            //kiểm tra xem mã userID đã được theo dõi chưa
            Following find = context.Followings.FirstOrDefault(p => p.FollowerId ==
            userID && p.FolloweeId == follow.FolloweeId);
            if (find != null)
            {
                //return BadRequest("The already following exists!");
                context.Followings.Remove(context.Followings.SingleOrDefault(p =>
 p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                context.SaveChanges();
                return Ok("cancel");
            }
            //set object follow
            follow.FollowerId = userID;
            context.Followings.Add(follow);
            context.SaveChanges();
            return Ok();
        }

    }
}
