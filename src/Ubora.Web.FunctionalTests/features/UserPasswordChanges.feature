Feature: User password changes
    As a user
    I want to register an account and change my password

Background:
    Given I go to the website "/"

Scenario: Get the title of Ubora
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I register an account
    When I click on the element "a=Sign in/sign up"
    When I sign up as "change@password.com" first name "Change" last name "Password"
    Then I expect the title of the page "Create a profile - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=TestFirstName TestLastName" is visible

Scenario: I try to change my password to empty password
    When I click on the element "span=Menu"
    When I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Change password"
    Then I expect the title of the page "Change Password - UBORA"
    When I click on the element "button=Change password"
    Then I expect the element "span=The Old password field is required." is visible
    Then I expect the element "span=The New password field is required." is visible
    Then I expect the title of the page "Change Password - UBORA"

Scenario: I change my password
    When I click on the element "span=Menu"
    When I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Change password"
    Then I expect the title of the page "Change Password - UBORA"
    When I set value "Test12345" to the element "#OldPassword"
    When I set value "PasswordChanged123!" to the element "#NewPassword"
    When I set value "PasswordChanged123!" to the element "#ConfirmPassword"
    When I click on the element "button=Change password"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I log out
    When I log out
    Then I expect the title of the page "Welcome - UBORA"
    Then I expect the element "a=Sign in/sign up" is visible

Scenario: I log in with my changed password
    When I sign in as "change@password.com" with password "PasswordChanged123!"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=Change Password" is visible
