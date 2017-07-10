Feature: Menu Options page
    As a signed in user
    I want to click Menu button and on all options in the Menu

Background:
    Given I go to the website "/Home/Index"

Scenario: Go to UBORA home page 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I sign up an account
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"
    When I set value "Micky" to the element "#FirstName"
    When I set value "Mouse" to the element "#LastName"
    When I set value "Micky.Mouse@email.com" to the element "#Email"
    When I set value "Test12345" to the element "#Password"
    When I set value "Test12345" to the element "#ConfirmPassword"
    When I click on the element "#IsAgreedToTermsOfService"
    When I click on the element "button=Create an account"
    Then I expect the title of the page "Create a profile page - UBORA"
    When I click on the element "a=Skip profile creation"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click View profile
    When I click on the element "span=Menu"
    When I click on the element "=View profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I click My projects
    When I click on the element "span=Menu"
    When I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"

Scenario: I click New projects
    When I click on the element "span=Menu"
    When I click on the element "i=create_new_folder"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I click Messages
    When I click on the element "span=Menu"
    When I click on the element "i=message"
    Then I expect the title of the page "Messages - UBORA"

Scenario: I click Notifications
    When I click on the element "span=Menu"
    When I click on the element "i=notifications"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "i=history"
    Then I expect the title of the page "Notification history - UBORA"
    When I click on the element ".notifications_pending-button"
    Then I expect the title of the page "Notifications - UBORA"

Scenario: I click Log out
    When I click on the element "span=Menu"
    When I click on the element "button=Log out"
    Then I expect the title of the page "Welcome - UBORA"