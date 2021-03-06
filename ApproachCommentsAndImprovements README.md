**Total time spent on the task: around 21 hours**

The task is very generally defined, as such it can be accomplished in 5 hours or 2 weeks. As my time to work on it was limited I'll mention the places which could be improved.

# General approach on my work

I went with following the structure of the existing code as it had everything needed already to improve the logic. First worked on the tasks, then refactoring, tests, and finally added comments. My approach in production is per feature and follows: code, refactor, comments, tests and repeat.

# Objective 1 - Buyer can see that offer is accepted

Questions that I would ask while reading the description and working on the feature:
- Should the buyer be alerted if he has the site open (push notifications)?
- Should some indication for that be added on the home page (i.e. dashboard)?
- Can buyer place another offer on the same property after his was accepted?
- Should the buyer be able to see all his offers and their statuses?
- Can the buyer place another offer after his current was rejected or can he put multiple orders on the same property?

Decided to show all offers in separate page as this covers the objective and gives clear picture to the buyer what's the status of all his offers.

Improvements on that approach:
- Add paging on the offers page
- Add filters for showing only accepted/rejected/pending orders
- Update properties page to show when offer has been made on property so that the buyer doesn't get confused (by adding duplicate)
- Show some indication if someone has made better offer, that would promote bidding competition between buyers

# Objective 2 - Buyer can book appointment

Questions that I would ask:
- What are the limits when it comes to booking date? For example: Should the seller specify his availability and the buyer be able to select only from those times. What if another buyer requested booking at the same time, should another booking be allowed at the same time? More questions would follow based on answers.
- What happens when appointment gets rejected from seller, can he do that? - Can buyer retract/change his appointment? What happens on Seller side then?
- Should seller see some notification that appointment was requested (menu/home page)? Same goes for buyer when appointment status is updated.
- Except booking date, what other information needs to be specified so that buyer and seller can meet?
- What happens with appointment when it's date passes and it wasn't accepted or declined?
- Time zones. The buyer and seller time zones might differ, when specifying the appointment date the buyer should specify the time zone also? The Seller on other hand should see the time in his time zone?

This task at the core is the same as offer functionality so I decided to do it in the same way. 

To show that I'm comfortable with JS and custom controls I decided to add date and time picker control to save time for the users typing the full date and time. At the same time, I left the possibility to manually enter the date as that might be easier for mobile users where the control could go outside screen bounds. 

Placed the booking functionality right in the properties page because there is no other data that needs to be specified, as such separate page didn't make sense.

Added appointments page per property similar to offers for Sellers and added appointments page to Buyers similar to offers.

Improvements on that approach:
- Add message specifying that appointment was created (for buyers). Ideally make the whole functionality with Ajax so there is no page reload.
- Show indication that buyer has already made appointment for property (on properties page)
- Add paging and filters for Buyers appointments page.

# Other improvements which could be done

- Add validation on search field in properties page.
- Add test cases to appointment and Buyer/Seller home pages functionality.

