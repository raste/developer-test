# Existing code review

The structure of the solution is good, with well-defined places for business logic, views, css, javascript and so on. The naming of the builders, commands, and handlers are clear in a way that comments for them are not needed.

Couple of things that I would do differently for production grade code:
- Put Javascript, Fonts, and CSS under one folder named Content as these are mostly updated simultaneously, for me it makes better sense to have them in one place. However, the packages do not follow that structure and that would-be a minus in this case.
- The subfolders for each Controller create good separation of the logic, however the way they are defined means that the logic for one action might get split between 2 or 3 folders. This structure is good for small number of classes, however I would prefer to have each action having it's own folder as in the long term that scales better and makes maintenance easier.
- As there are 2 distinct types of users (Buyers and Sellers) with their own features, reusing the same controls for both is bound to get messy from maintenance perspective in the long term, not to mention that they won't be able to reuse the same action names without adding additional complexity in the views. As such it's best to split their functionality in 2 sub portals via areas in MVC. The end result will add /Buyers/... and /Sellers/... in the URLs specific for each type of user. Common functionality can reside in /Controllers folder as it currently is.
- I see that Property controller ListForSale does not validate if the property belongs to the current seller, so that is security hole. 
- Property/MakeOffer doesn't check if the property is listed for sale. I'm guilty of the same since I didn't add such check to Appointment boking functionality also.
